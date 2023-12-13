using System;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            InitGame();
        }
        
        public static GameManager Instance => _instance;

        #endregion

        private GameData _gameData;
        
        public GameData GameData => _gameData;

        private void Start()
        {
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
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