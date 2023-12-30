using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PopupReward : MonoBehaviour
    {
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image iconReward;

        [Header("Anim")]
        [SerializeField] private float animDuration = 0.2f;
        
        public void Show(string description, Sprite icon)
        {
            this.description.text = description;
            iconReward.sprite = icon;
            
            
            this.DOKill();
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;

            transform.DOScale(1f, animDuration).SetTarget(this);
        }

        public void Hide()
        {
            this.DOKill();
            transform.DOScale(0f, animDuration).SetTarget(this).OnComplete( () =>gameObject.SetActive(false));
        }
    }
}