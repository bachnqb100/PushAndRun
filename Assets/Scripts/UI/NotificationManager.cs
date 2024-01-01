using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class NotificationManager : MonoBehaviour
    {
        #region Singleton

        private static NotificationManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static NotificationManager Instance => _instance;

        #endregion

        [SerializeField] private Transform container;
        [SerializeField] private TMP_Text context;
        [SerializeField] private float animShowDuration = 0.2f;
        [SerializeField] private float animHideDuration = 2f;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private float showInterval = 2f;

        private void Awake()
        {
            InitSingleton();
            container.gameObject.SetActive(false);
        }

        [Button]
        public void ShowNotification(string message)
        {
            this.DOKill();
            container.gameObject.SetActive(true);
            context.text = message;
            canvasGroup.alpha = 1f;
            container.localScale = Vector3.zero;

            container.transform.DOScale(1f, animShowDuration).OnComplete(() =>
            {
                Hide();
            }).SetTarget(this);

        }

        void Hide()
        {
            DOVirtual.DelayedCall(showInterval, () =>
            {
                DOVirtual.Float(1f, 0f, animHideDuration, x =>
                {
                    canvasGroup.alpha = x;
                }).OnComplete(() => container.gameObject.SetActive(false)).SetTarget(this);
            }).SetTarget(this);
        }
    }
}