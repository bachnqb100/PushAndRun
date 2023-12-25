using System;
using DG.Tweening;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.UI
{
    public class CountDown : MonoBehaviour
    {
        [SerializeField] private float countDownValue = 3f;

        [SerializeField] private TMP_Text countDownText;

        [SerializeField] private CanvasGroup canvasGroup;

        public Relay OnCompleteCountDown = new Relay(); 

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            StartCountDown();
        }

        void StartCountDown()
        {
            this.DOKill();
            canvasGroup.alpha = 1f;
            DOVirtual.Float(countDownValue, 1f, countDownValue - 1f, x =>
            {
                countDownText.text = ((int)x).ToString();
            }).OnComplete(() =>
            {
                countDownText.text = "Start";
                DOVirtual.Float(1f, 0f, 1f, x =>
                {
                    canvasGroup.alpha = x;
                }).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnCompleteCountDown.Dispatch();
                }).SetTarget(this);
            }).SetTarget(this);
        }
    }
}