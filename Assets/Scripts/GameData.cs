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

            
            public DateTime lastTimeLogOut = DateTime.Now;
            
            public float currentSpeed = 1f;
            public float fitness = 5f;
            public float fitnessDecreaseRate = 1f;
            public float fitnessIncreaseRate = 1f;

            public int moneyIncome = 5;
            
            
            //skin
            public SerializedDictionary<ClothesType, ClothesColorType> clothesColorMap =
                new SerializedDictionary<ClothesType, ClothesColorType>();
            
            
            public SerializedDictionary<ClothesType, int> test =
                new SerializedDictionary<ClothesType, int>();


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