using System;
using DefaultNamespace.Configs;
using DefaultNamespace.Skin;
using DefaultNamespace.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static GameManager Instance => _instance;

        #endregion

        public GameData _gameData;
        
        public GameData GameData => _gameData;

        #region Extra Futures

        public float LastClaimOnlineGiftTime { get; set; }

        #endregion
        
        
        private void Awake()
        {
            InitSingleton();

            InitGame();
        }

        private void Start()
        {
            _gameData.userData.ValidateData();
            
            if (_gameData.userData.inited)
            {
                SkinManager.Instance.SetColorPlayer();
            }
            else
            {
                _gameData.userData.inited = true;
            }
        }

        void InitGame()
        {
            LoadGameDataFromLocalDatabase();

        }
        

        void LoadGameDataFromLocalDatabase()
        {
            _gameData = LocalDatabase.GetGameData();

            if (_gameData == null)
            {
                _gameData = new GameData();
                
                LocalDatabase.SaveData();
            }
        }
        
        
        
        [Button]
        public void AddMoney(int value)
        {
            _gameData.userData.money += value;
            EventGlobalManager.Instance.OnMoneyChange.Dispatch(true);
        }

        [Button]
        public bool SpendMoney(int value)
        {
            if (_gameData.userData.money >= value)
            {
                _gameData.userData.money -= value;
                EventGlobalManager.Instance.OnMoneyChange.Dispatch(true);
                return true;
            }

            EventGlobalManager.Instance.OnMoneyChange.Dispatch(false);
            return false;
        }
        
        public void OnApplicationQuit()
        {
            Logout();
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                Logout();
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                Logout();
        }

        private void Logout()
        {
            _gameData.userData.lastTimeLogOut = DateTime.Now;
            LocalDatabase.SaveData();
        }

        #region Extra Features

        public static int GetRemainTime() => Mathf.RoundToInt(ConfigManager.Instance.extraFeaturesConfig.onlineRewardInterval -
                                                              (Time.time - GameManager.Instance.LastClaimOnlineGiftTime));

        #endregion

    }
}