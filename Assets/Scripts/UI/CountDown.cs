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
            
            DOVirtual.Float(countDownValue, 0f, countDownValue, x =>
            {
                countDownText.text = ((int)x).ToString();
            }).OnComplete(() =>
            {
                gameObject.SetActive(false);
                OnCompleteCountDown.Dispatch();
            }).SetTarget(this);
        }
    }
}