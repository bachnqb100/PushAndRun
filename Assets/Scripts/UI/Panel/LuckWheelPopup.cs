using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Audio;
using DefaultNamespace.Configs;
using DefaultNamespace.Haptic;
using DefaultNamespace.UI.LuckyWheel;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace.UI
{
    public class LuckWheelPopup : UIPanel
    {
        [SerializeField] private List<WheelItem> wheelItems;
        [SerializeField] private Transform spin;
        [SerializeField] private ButtonExtension spinBtn;
        [SerializeField] private TMP_Text spinBtnLabel;
        [SerializeField, Range(0, 100)] private float giftRate;
        [SerializeField] private LuckyWheelProgress luckyWheelProgress;
        [SerializeField] private ButtonExtension spinAdsBtn;

        [SerializeField] private ButtonExtension closeBtn;

        private bool _isSpinning;    
        private int _giftRewardIndex;

        protected override void Init()
        {
            base.Init();
            
            _isSpinning = false;
            spinBtn.interactable = false;
            
            InitItems();
            UpdateSpinBtn();
            luckyWheelProgress.Init();
        }
        
        void InitItems()
        {
            _giftRewardIndex = Random.Range(0, wheelItems.Count);

            List<int> coinRewards = ConfigManager.Instance.extraFeaturesConfig.luckyWheelCoinRewardValues.Clone();

            for (int i = 0; i < wheelItems.Count; i++)
            {
                if (_giftRewardIndex == i)
                {
                    wheelItems[i].InitGift();
                }
                else
                {
                    int reward = coinRewards.GetRandom();
                    coinRewards.Remove(reward);
                    wheelItems[i].InitCoin(reward);
                }
            }
        }

        public void FreeSpin()
        {
            if (!_isSpinning)
            {
                Spin();
                spinBtn.interactable = false;
                GameManager.Instance.GameData.userData.lastFreeSpinTime = DateTime.Now;
            }
        }

        public void SpinAds()
        {
            if (!_isSpinning)
            {
                Spin();
            }
        }
        
        IEnumerator SpinSound()
        {
            for (int i = 0; i < 9; i++)
            {
                AudioAssistant.Shot(TypeSound.Spin);
                yield return Yielders.Get(0.5f);
            }
        }

        [Button]
        void Spin()
        {
            BHHaptic.Haptic(HapticTypes.MediumImpact);
            
            _isSpinning = true;
            
            if (GameManager.Instance.GameData.userData.luckyWheelProgress == 10)
                GameManager.Instance.GameData.userData.luckyWheelProgress = 0;

            GameManager.Instance.GameData.userData.luckyWheelProgress++;
            luckyWheelProgress.UpdateProgress();
            
            int rd = (Random.Range(0f, 100f) < giftRate ? _giftRewardIndex : GetRandomCoinItemIndex())
                     + wheelItems.Count * Random.Range(5, 8);
            var rewardItem = wheelItems[rd % wheelItems.Count];
            spin.DORotate(new Vector3(0, 0, -rd * 45), 5).SetEase(Ease.OutQuad).SetRelative(true).OnComplete(() =>
            {
                _isSpinning = false;

                rewardItem.Claim();
            });

            StartCoroutine(SpinSound());
        }

        void UpdateSpinBtn()
        {
            if (spinBtn.interactable)
                return;

            var remainTime = GameManager.GetRemainTimeWheel();

            if (remainTime >= 0)
                spinBtnLabel.text = remainTime.ToTimeFormat();
            else
            {
                spinBtnLabel.text = "Spin";
                spinBtn.interactable = true;
            }
        }
        
        int GetRandomCoinItemIndex()
        {
            int result = Random.Range(0, wheelItems.Count);

            while (result == _giftRewardIndex)
                result = Random.Range(0, wheelItems.Count);

            return result;
        }
        

        protected override void RegisterEvent()
        {
            base.RegisterEvent();

            spinBtn.onClick.AddListener(FreeSpin);
            spinAdsBtn.onClick.AddListener(SpinAds);
            closeBtn.onClick.AddListener(ClosePopup);
            
            EventGlobalManager.Instance.OnEverySecondTick.AddListener(UpdateSpinBtn);
            
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            spinBtn.onClick.RemoveListener(FreeSpin);
            spinAdsBtn.onClick.RemoveListener(SpinAds);
            closeBtn.onClick.RemoveListener(ClosePopup);
            
            if (EventGlobalManager.Instance)
                EventGlobalManager.Instance.OnEverySecondTick.RemoveListener(UpdateSpinBtn);
        }

        void ClosePopup()
        {
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }
    }
}