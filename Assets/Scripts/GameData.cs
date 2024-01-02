using System;
using System.Collections.Generic;
using DefaultNamespace.Configs;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class GameData
    {
        public SettingData setting = new SettingData();
        public UserData userData = new UserData(); 
        
        [Serializable]
        public class SettingData
        {
            public bool haptic = true;

            public float bgMusicVolume = 1f;

            public float sfxVolume = 1f;
        }

        [Serializable]
        public class UserData
        {
            public bool inited = false;
            
            public string username = "Player";
            public int money = 200;

            public int map = 0;

            
            public DateTime lastTimeLogOut = DateTime.Now;
            
            public int luckyWheelProgress;
            public DateTime lastFreeSpinTime = DateTime.MinValue;
            
            public int dailyRewardClaimedCount;
            public DateTime lastDailyRewardClaimTime = DateTime.MinValue;
            
            public float currentSpeed = 1f;
            public float fitness = 5f;
            public float fitnessDecreaseRate = 1f;
            public float fitnessIncreaseRate = 1f;

            public int moneyIncome = 5;
            
            //gift
            public MainAnimType currentDailyReward = MainAnimType.SlowRun;
            public VictoryAnimType currentWheelReward = VictoryAnimType.BellyDance;
            
            
            //skin
            public SerializedDictionary<ClothesType, ClothesColorType> clothesColorMap =
                new SerializedDictionary<ClothesType, ClothesColorType>();
            
            
            public SerializedDictionary<ClothesType, int> test =
                new SerializedDictionary<ClothesType, int>();
            
            //shop
            public SerializedDictionary<MainAnimType, int> mainAnimItemPriceMap =
                new SerializedDictionary<MainAnimType, int>();

            public SerializedDictionary<MainAnimType, bool> mainAnimItemStatusMap =
                new SerializedDictionary<MainAnimType, bool>();

            public MainAnimType currentMainAnimType = MainAnimType.SlowRun;
 
            
            public SerializedDictionary<VictoryAnimType, int> victoryAnimItemPriceMap =
                new SerializedDictionary<VictoryAnimType, int>();

            public SerializedDictionary<VictoryAnimType, bool> victoryAnimItemStatusMap =
                new SerializedDictionary<VictoryAnimType, bool>();

            public VictoryAnimType currentVictoryAnimType = VictoryAnimType.BellyDance;
            
            //tutorial
            public bool isFirstMovement = true;
            public bool isFirstDestination = true;


            public void ValidateData()
            {
                for (int i = 0; i <= Enum.GetValues(typeof(ClothesType)).Length - 1; i++)
                {
                    var type = (ClothesType)i;
                    ClothesColorType? value 
                        = ConfigManager.Instance.skinConfig.GetClothesColorType(type);
                    if (value != null)
                        clothesColorMap.TryAdd(type, value.Value);
                }


                for (int i = 0; i < Enum.GetValues(typeof(MainAnimType)).Length; i++)
                {
                    var type = (MainAnimType)i;
                    mainAnimItemStatusMap.TryAdd(type, type == MainAnimType.SlowRun);
                }
                
                for (int i = 0; i < Enum.GetValues(typeof(MainAnimType)).Length; i++)
                {
                    var type = (MainAnimType)i;
                    var price = ConfigManager.Instance.animMainGroupConfig.GetAnimMainConfig(type).price;
                    mainAnimItemPriceMap.TryAdd(type, type == MainAnimType.SlowRun ? 0 : price);
                }
                
                
                for (int i = 0; i < Enum.GetValues(typeof(VictoryAnimType)).Length; i++)
                {
                    var type = (VictoryAnimType)i;
                    victoryAnimItemStatusMap.TryAdd(type, type == VictoryAnimType.BellyDance);
                }
                
                for (int i = 0; i < Enum.GetValues(typeof(VictoryAnimType)).Length; i++)
                {
                    var type = (VictoryAnimType)i;
                    var price = ConfigManager.Instance.animVictoryGroupConfig.GetAnimVictoryConfig(type).price;
                    victoryAnimItemPriceMap.TryAdd(type, type == VictoryAnimType.BellyDance ? 0 : price);
                }
            }
        }
    }

    public static class LocalDatabase
    {
        private static string dataKey = "GameData";

        public static void SaveData()
        {
            var dataString = JsonConvert.SerializeObject(GameManager.Instance.GameData);
            PlayerPrefs.SetString(dataKey, dataString);
            PlayerPrefs.Save();
        }

        public static GameData GetGameData()
        {
            if (!PlayerPrefs.HasKey(dataKey))
                return null;
            
            return JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(dataKey));
        }
    }
}