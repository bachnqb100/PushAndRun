using System;
using System.Collections;
using DefaultNamespace.Audio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class BloodScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float animDuration = 1f;

        private Coroutine _sound;
        [Button]
        public void SetStatusAnimBloodScreen(bool enable)
        {
            canvasGroup.DOKill();
            
            if (!enable)
            {
                DOVirtual.Float(canvasGroup.alpha, 0f, animDuration, x =>
                {
                    canvasGroup.alpha = x;
                }).OnComplete(() => gameObject.SetActive(false)).SetTarget(canvasGroup);
                
                if (_sound != null)
                    StopCoroutine(_sound);
                
                return;
            }

            gameObject.SetActive(true);
            _sound = StartCoroutine(ShotAudio());
            canvasGroup.alpha = 0f;
            DOVirtual.Float(0f, 1f, animDuration, x =>
            {
                canvasGroup.alpha = x;
            }).SetLoops(-1, LoopType.Yoyo).SetTarget(this);
        }

        IEnumerator ShotAudio()
        {
            float timeDuration = (float) Math.Round(AudioAssistant.GetLength(TypeSound.WarningDetect), 1);

            while (true)
            {
                AudioAssistant.Shot(TypeSound.WarningDetect);
                yield return Yielders.Get(timeDuration);
            }
        }
    }
}