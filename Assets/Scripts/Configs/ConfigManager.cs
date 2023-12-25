using System;
using DefaultNamespace.Configs.Skin;
using UnityEngine;

namespace DefaultNamespace.Configs
{
    public class ConfigManager : MonoBehaviour
    {
        #region Singleton

        private static ConfigManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }

        public static ConfigManager Instance => _instance;

        #endregion
        
        public SoundConfig soundConfig;

        public SkinConfig skinConfig;

        private void Awake()
        {
            InitSingleton();
        }
    }
}