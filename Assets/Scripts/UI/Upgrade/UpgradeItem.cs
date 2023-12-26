using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.Upgrade
{
    public class UpgradeItem : MonoBehaviour
    {
        [SerializeField] private UpgradeType upgradeType;

        [SerializeField]
        private TMP_Text upgradeName, upgradeCurrentValue, upgradeNextValue, upgradeDescription, upgradeMoney;

        [SerializeField] private ButtonExtension btnUpgrade; 
        
        [SerializeField] private Image btnUpgradeImage;
        [SerializeField] private Sprite bgCanUpgradeSprite, bgNoUpgradeSprite;
        private void OnEnable()
        {
            btnUpgrade.onClick.AddListener(Upgrade);

            EventGlobalManager.Instance.OnUpdateDataUpgrade.AddListener(UpdateData);
        }

        private void OnDisable()
        {
            btnUpgrade.onClick.RemoveListener(Upgrade);

            EventGlobalManager.Instance.OnUpdateDataUpgrade.RemoveListener(UpdateData);
        }

        private void Start()
        {
            Init();
        }

        void Init()
        {
            upgradeName.text = UpgradeManager.Instance.GetUpgradeName(upgradeType);
            upgradeDescription.text = UpgradeManager.Instance.GetUpgradeDescription(upgradeType);
            
            UpdateData();
        }

        void UpdateData()
        {
            upgradeMoney.text = UpgradeManager.Instance.GetMoneyUpgrade(upgradeType).ToString();
            upgradeCurrentValue.text = UpgradeManager.Instance.GetCurrentData(upgradeType);
            upgradeNextValue.text = UpgradeManager.Instance.GetNextData(upgradeType);

            btnUpgradeImage.sprite = UpgradeManager.Instance.CanUpgradeStat(upgradeType)
                ? bgCanUpgradeSprite
                : bgNoUpgradeSprite;

            btnUpgrade.interactable = UpgradeManager.Instance.CanUpgradeStat(upgradeType);
        }

        void Upgrade()
        {
            UpgradeManager.Instance.Upgrade(upgradeType);
            
            EventGlobalManager.Instance.OnUpdateDataUpgrade.Dispatch();
        }
    }
}