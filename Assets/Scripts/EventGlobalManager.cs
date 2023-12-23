using System;
using Sigtrap.Relays;
using UnityEngine;

namespace DefaultNamespace
{
    public class EventGlobalManager : MonoBehaviour
    {
        #region Singleton

        private static EventGlobalManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }

        public static EventGlobalManager Instance => _instance;

        #endregion

        private void Awake()
        {
            InitSingleton();
        }

        public Relay OnUpdateSetting = new Relay();
        public Relay<bool> OnMoneyChange = new Relay<bool>();
        public Relay OnPlayerStartSprint = new Relay();
        public Relay OnPlayerEndSprint = new Relay();
        public Relay<bool> OnPlayerJog = new Relay<bool>();
        public Relay<float> OnUpdateFitness = new Relay<float>();
    }
}