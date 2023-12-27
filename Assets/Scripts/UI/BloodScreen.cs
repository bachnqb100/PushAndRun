using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class BloodScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float animDuration = 1f;

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
                return;
            }
            
            gameObject.SetActive(true);
            canvasGroup.alpha = 0f;
            DOVirtual.Float(0f, 1f, animDuration, x =>
            {
                canvasGroup.alpha = x;
            }).SetLoops(-1, LoopType.Yoyo).SetTarget(this);
        }
    }
}