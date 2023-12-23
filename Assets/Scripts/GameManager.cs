using System;
using DefaultNamespace.UI;
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

        private GameData _gameData;
        
        public GameData GameData => _gameData;
        
        
        private void Awake()
        {
            InitSingleton();

            InitGame();
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

    }
}