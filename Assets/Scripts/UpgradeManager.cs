using System;
using DefaultNamespace.Configs.Upgrade;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class UpgradeManager : MonoBehaviour
    {
        #region Singleton

        private static UpgradeManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static UpgradeManager Instance => _instance;

        #endregion

        [SerializeField] private List<UpgradeItemConfig> itemConfigs;
        
        private SerializedDictionary<UpgradeType, UpgradeItemConfig> _upgradeItemMap =
            new SerializedDictionary<UpgradeType, UpgradeItemConfig>();


        private void Awake()
        {
            InitSingleton();
            
            LoadConfig();
        }

        void LoadConfig()
        {
            _upgradeItemMap.Clear();
            foreach (var item in itemConfigs)
            {
                _upgradeItemMap.TryAdd(item.type, item);
            }
        }

        public void Upgrade(UpgradeType type)
        {
            if (!GameManager.Instance.SpendMoney(_upgradeItemMap[type].moneyUpgrade)) return;
            
            switch (type)
            {
                case UpgradeType.Speed:
                    GameManager.Instance.GameData.userData.currentSpeed +=
                        _upgradeItemMap[type].valueType == UpgradeValueType.Float
                            ? _upgradeItemMap[type].upgradeFloatValue
                            : _upgradeItemMap[type].upgradeIntValue; 
                    break;
                case UpgradeType.Fitness:
                    GameManager.Instance.GameData.userData.fitness +=
                        _upgradeItemMap[type].valueType == UpgradeValueType.Float
                            ? _upgradeItemMap[type].upgradeFloatValue
                            : _upgradeItemMap[type].upgradeIntValue;
                    break;
                case UpgradeType.RecoveryFitness:
                    GameManager.Instance.GameData.userData.fitnessIncreaseRate +=
                        _upgradeItemMap[type].valueType == UpgradeValueType.Float
                            ? _upgradeItemMap[type].upgradeFloatValue
                            : _upgradeItemMap[type].upgradeIntValue;
                    break;
                case UpgradeType.ConsumeFitness:
                    GameManager.Instance.GameData.userData.fitnessDecreaseRate +=
                        _upgradeItemMap[type].valueType == UpgradeValueType.Float
                            ? _upgradeItemMap[type].upgradeFloatValue
                            : _upgradeItemMap[type].upgradeIntValue;
                    break;
                case UpgradeType.MoneyIncome:
                    GameManager.Instance.GameData.userData.moneyIncome += (int)
                        (_upgradeItemMap[type].valueType == UpgradeValueType.Float
                            ? _upgradeItemMap[type].upgradeFloatValue
                            : _upgradeItemMap[type].upgradeIntValue);
                    break;
            }
        }
        
        public T GetNextData<T>(UpgradeType type) where T : struct, IComparable, IFormattable, IConvertible
        {
            switch (type)
            {
                case UpgradeType.Speed:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.currentSpeed +
                                                 (_upgradeItemMap[type].valueType == UpgradeValueType.Float
                                                     ? _upgradeItemMap[type].upgradeFloatValue
                                                     : _upgradeItemMap[type].upgradeIntValue), typeof(T));
                case UpgradeType.Fitness:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.fitness +
                                                 (_upgradeItemMap[type].valueType == UpgradeValueType.Float
                                                     ? _upgradeItemMap[type].upgradeFloatValue
                                                     : _upgradeItemMap[type].upgradeIntValue), typeof(T));
                case UpgradeType.RecoveryFitness:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.fitnessIncreaseRate +
                                                 (_upgradeItemMap[type].valueType == UpgradeValueType.Float
                                                     ? _upgradeItemMap[type].upgradeFloatValue
                                                     : _upgradeItemMap[type].upgradeIntValue), typeof(T));
                case UpgradeType.ConsumeFitness:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.fitnessDecreaseRate +
                                                 (_upgradeItemMap[type].valueType == UpgradeValueType.Float
                                                     ? _upgradeItemMap[type].upgradeFloatValue
                                                     : _upgradeItemMap[type].upgradeIntValue), typeof(T));
                case UpgradeType.MoneyIncome:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.moneyIncome +
                                                 (_upgradeItemMap[type].valueType == UpgradeValueType.Float
                                                     ? _upgradeItemMap[type].upgradeFloatValue
                                                     : _upgradeItemMap[type].upgradeIntValue), typeof(T));
            }

            return default(T);
        }

        public T GetCurrentData<T>(UpgradeType type) where T : struct, IComparable, IFormattable, IConvertible
        {
            switch (type)
            {
                case UpgradeType.Speed:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.currentSpeed, typeof(T));
                case UpgradeType.Fitness:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.fitness, typeof(T));
                case UpgradeType.RecoveryFitness:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.fitnessIncreaseRate, typeof(T));
                case UpgradeType.ConsumeFitness:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.fitnessDecreaseRate, typeof(T));
                case UpgradeType.MoneyIncome:
                    return (T)Convert.ChangeType(GameManager.Instance.GameData.userData.moneyIncome, typeof(T));
            }

            return default(T);
        }
        
        
        public string GetCurrentData (UpgradeType type) 
        {
            switch (type)
            {
                case UpgradeType.Speed:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.currentSpeed);
                case UpgradeType.Fitness:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.fitness);
                case UpgradeType.RecoveryFitness:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.fitnessIncreaseRate);
                case UpgradeType.ConsumeFitness:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.fitnessDecreaseRate);
                case UpgradeType.MoneyIncome:
                    return GameManager.Instance.GameData.userData.moneyIncome.ToString();
            }

            return default(string);
        }
        
        
        public string GetNextData (UpgradeType type) 
        {
            switch (type)
            {
                case UpgradeType.Speed:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.currentSpeed + _upgradeItemMap[type].upgradeFloatValue);
                case UpgradeType.Fitness:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.fitness + _upgradeItemMap[type].upgradeFloatValue);
                case UpgradeType.RecoveryFitness:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.fitnessIncreaseRate + _upgradeItemMap[type].upgradeFloatValue);
                case UpgradeType.ConsumeFitness:
                    return ToStringFormatFloat(GameManager.Instance.GameData.userData.fitnessDecreaseRate + _upgradeItemMap[type].upgradeFloatValue);
                case UpgradeType.MoneyIncome:
                    return (GameManager.Instance.GameData.userData.moneyIncome + _upgradeItemMap[type].upgradeIntValue).ToString();
            }

            return default(string);
        }

        string ToStringFormatFloat(float number) => number.ToString("0.00");

        public int GetMoneyUpgrade(UpgradeType type)
        {
            return _upgradeItemMap[type].moneyUpgrade;
        }
        
        public string GetUpgradeName(UpgradeType type) => _upgradeItemMap[type].nameUpgrade;

        public string GetUpgradeDescription(UpgradeType type) => _upgradeItemMap[type].description;

        public bool CanUpgradeStat(UpgradeType type)
        {
            if (GameManager.Instance.GameData.userData.money >= _upgradeItemMap[type].moneyUpgrade) return true;
            return false;
        }

    }
}