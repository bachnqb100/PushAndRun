using System;
using Sigtrap.Relays;
using UnityEngine;

namespace DefaultNamespace
{
    public class EventGlobalManager : MonoBehaviour
    {
        #region Singleton

        private static EventGlobalManager _instance;

        private void Awake()
        {
            if (_instance)
                _instance = this;
        }

        public static EventGlobalManager Instance => _instance;

        #endregion

        public Relay OnUpdateSetting = new Relay();
    }
}