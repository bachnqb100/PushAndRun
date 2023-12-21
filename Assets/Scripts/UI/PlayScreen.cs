using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


namespace DefaultNamespace.UI
{
    public class PlayScreen : UIPanel
    {
        [Header("Timer")] 
        [SerializeField] private TMP_Text timer;

        [SerializeField] private float warningTimeUpScale = 1.5f;
        [SerializeField] private float warningTimeUpDuration = 0.5f;

        [SerializeField] private Vector3 warningTimeUpShake = new Vector3(0f, 0f, 10f);
        [SerializeField] private float warningTimeUpShakeDuration = 1f;


        [Header("Control")] 
        [SerializeField] private CountDown countDown;

        [SerializeField] private List<GameObject> controls;


        private bool _isWarningTimeUp;

        public override void Show(Action action = null)
        {
            base.Show(action);

            _isWarningTimeUp = false;
            this.DOKill();
            
            HideControl();
            countDown.gameObject.SetActive(true);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();

            countDown.OnCompleteCountDown.AddListener(ShowControl);
            countDown.OnCompleteCountDown.AddListener(StartGame);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();

            countDown.OnCompleteCountDown.RemoveListener(ShowControl);
            countDown.OnCompleteCountDown.RemoveListener(StartGame);
        }

        private void Update()
        {
            UpdateTime();
        }

        void HideControl()
        {
            foreach (var controlItem in controls)
            {
                controlItem.SetActive(false);
            }
        }

        void ShowControl()
        {
            foreach (var controlItem in controls)
            {
                controlItem.SetActive(true);
            }
        }

        void StartGame()
        {
            GameController.Instance.IsPlaying = true;
        }

        void UpdateTime()
        {
            var timeLeft = (int) GameController.Instance.TimeLeft;

            timer.text = timeLeft.ToTimeFormatCompact();
            
            // time left warning
            if (timeLeft <= 5)
            {
                WarningTimeUp();
            }
        }
        
        void WarningTimeUp()
        {
            if (_isWarningTimeUp) return;
            _isWarningTimeUp = true;

            timer.transform.DOScale(warningTimeUpScale, warningTimeUpDuration).SetLoops(-1, LoopType.Yoyo)
                .SetTarget(this);
            timer.transform.DOShakeRotation(warningTimeUpShakeDuration, warningTimeUpShake).SetLoops(-1, LoopType.Yoyo)
                .SetTarget(this);
        } 
    }
}