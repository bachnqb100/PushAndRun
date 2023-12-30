using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace.UI.Daily_Reward
{
    public class Notify : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float showDuration = 1f;
        [SerializeField] private float duration = 1.5f;

        public void Show()
        {
            canvasGroup.DOKill(true);
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;

            DOVirtual.DelayedCall(showDuration, () =>
            {
                canvasGroup.DOFade(0f, duration).OnComplete(() => gameObject.SetActive(false)).SetTarget(canvasGroup);
            }).SetTarget(canvasGroup);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}