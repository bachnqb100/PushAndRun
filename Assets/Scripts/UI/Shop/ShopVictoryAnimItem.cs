using System;
using DefaultNamespace.Audio;
using DefaultNamespace.Configs;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.Shop
{
    public class ShopVictoryAnimItem : MonoBehaviour
    {
        [Header("Background")] 
        [SerializeField] private Image bgImage;
        [SerializeField] private Sprite bgSpriteNormal, bgSpriteAds, bgSpriteExclusive;
        
        [SerializeField] private TMP_Text nameItem;
        [SerializeField] private Image iconItem;

        [Header("Unlock")]
        [SerializeField] private TMP_Text priceMoney;
        [SerializeField] private TMP_Text priceAds;
        [SerializeField] private ButtonExtension btnUse, btnMoney, btnAds, btnSelect;

        [Header("Exclusive")] 
        [SerializeField] private GameObject exclusive;
        [SerializeField] private TMP_Text exclusiveDescription;
        
        [Header("Selected")]
        [SerializeField] private GameObject selected;
        [SerializeField] private GameObject selectedUse;

        private AnimVictoryConfig _config;
        private Action _onSelect;
        private Action _onUse;

        private void OnEnable()
        {
            btnSelect.onClick.AddListener(Select);
            btnUse.onClick.AddListener(Use);
            btnMoney.onClick.AddListener(Buy);
            btnAds.onClick.AddListener(Buy);
        }

        private void OnDisable()
        {
            btnSelect.onClick.RemoveListener(Select);
            btnUse.onClick.RemoveListener(Use);
            btnMoney.onClick.RemoveListener(Buy);
            btnAds.onClick.RemoveListener(Buy);
        }

        [Button]
        public void Init(VictoryAnimType type, Action onSelect, Action onUse)
        {
            _onSelect = onSelect;
            _onUse = onUse;
            
            _config = ConfigManager.Instance.animVictoryGroupConfig.GetAnimVictoryConfig(type);

            if (_config)
            {
                nameItem.text = _config.animName;
                bgImage.sprite = _config.isExclusive ? bgSpriteExclusive :
                    _config.isAdsUnlock ? bgSpriteAds : bgSpriteNormal;

                iconItem.sprite = _config.icon;
                
                selected.SetActive(false);
                selectedUse.SetActive(false);
                
                //exclusive
                
                exclusive.SetActive(!GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[type] && _config.isExclusive);
                if (_config.isExclusive) exclusiveDescription.text = _config.description;
                
                //unlock
                UpdateData();
            }
        }

        void UpdateData()
        {
            btnUse.gameObject.SetActive(GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[_config.type]);
            btnAds.gameObject.SetActive(!GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[_config.type] && _config.isAdsUnlock);
            btnMoney.gameObject.SetActive(!GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[_config.type] &&
                                          !_config.isExclusive && !_config.isAdsUnlock);
                
            if (_config.isAdsUnlock)
                priceAds.text = GameManager.Instance.GameData.userData.victoryAnimItemPriceMap[_config.type].ToString() + "/" +
                                _config.price;
            else if (_config.isExclusive)
                priceMoney.text = _config.price.ToString();
        }

        public void DeSelect()
        {
            selected.SetActive(false);
        }

        void Select()
        {
            _onSelect?.Invoke();
            selected.SetActive(true);
            //TODO: Logic player on select
            
            GameController.Instance.Player.AnimController.SetAnimVictory(_config.type);
        }

        public void DeSelectUse()
        {
            selectedUse.SetActive(false);
            UpdateData();
        } 

        public void Use()
        {
            Select();
            
            _onUse?.Invoke();
            selectedUse.SetActive(true);
            btnUse.gameObject.SetActive(false);
            btnMoney.gameObject.SetActive(false);
            btnAds.gameObject.SetActive(false);
            
            //TODO: Logic Use
            GameManager.Instance.GameData.userData.currentVictoryAnimType = _config.type;
        }

        void Buy()
        {
            Select();

            if (_config.isAdsUnlock)
            {
                GameManager.Instance.GameData.userData.victoryAnimItemPriceMap[_config.type]--;
                if (GameManager.Instance.GameData.userData.victoryAnimItemPriceMap[_config.type] <= 0)
                {
                    GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[_config.type] = true;
                    
                    AudioAssistant.Shot(TypeSound.Unlock);
                    
                    UpdateData();
                    Use();
                    return;
                }
                UpdateData();
            }
            else
            {
                if (GameManager.Instance.GameData.userData.money >=
                    GameManager.Instance.GameData.userData.victoryAnimItemPriceMap[_config.type])
                {
                    GameManager.Instance.SpendMoney(
                        GameManager.Instance.GameData.userData.victoryAnimItemPriceMap[_config.type]);
                    GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[_config.type] = true;
                    
                    AudioAssistant.Shot(TypeSound.Unlock);
                    
                    UpdateData();
                    
                    Use();
                }
            }
            
        }
    }
}