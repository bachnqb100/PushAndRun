﻿using System;
using System.Collections;
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

        public Relay OnEverySecondTick = new Relay();

        public Relay<int> OnClaimMoney = new Relay<int>();

        public Relay<bool> OnPlayerJump = new Relay<bool>();

        public Relay OnUpdateSetting = new Relay();
        public Relay<bool> OnMoneyChange = new Relay<bool>();
        public Relay OnPlayerStartSprint = new Relay();
        public Relay OnPlayerEndSprint = new Relay();
        public Relay<bool> OnPlayerJog = new Relay<bool>();
        public Relay<float> OnUpdateFitness = new Relay<float>();

        //item
        public Relay<float> OnPlayerCollectShield = new Relay<float>();
        public Relay<float> OnPlayerCollectInvisible = new Relay<float>();
        public Relay OnEnemyKnockout = new Relay();
        public Relay<float> OnPlayerCollectRecoveryFitness = new Relay<float>();
        public Relay<float> OnPlayerCollectConsumeFitness = new Relay<float>();
        public Relay<float> OnPlayerExhausted = new Relay<float>();

        //upgrade
        public Relay OnUpdateDataUpgrade = new Relay();
        
        //blood screen
        public Relay<bool> OnUpdateBloodScreen = new Relay<bool>();
        
        private void Start()
        {
            StartCoroutine(EverySecondTick());
        }

        IEnumerator EverySecondTick()
        {
            while (true)
            {
                OnEverySecondTick.Dispatch();
                yield return Yielders.Get(1);
            }
        }
    }
}