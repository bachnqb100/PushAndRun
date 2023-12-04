using System;
using System.Collections.Generic;
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
            public bool hasBgMusic = true;

            public float sfxVolume = 1f;
            public bool hasSfxMusic = true;
        }

        [Serializable]
        public class UserData
        {
            public string username = "Player";
            public int money = 200;
            
            
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