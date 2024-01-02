using System;
using DefaultNamespace.Audio;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PopupReward : MonoBehaviour
    {
        #region Singleton

        private static PopupReward _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static PopupReward Instance => _instance;

        #endregion
        
        
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image iconReward;
        [SerializeField] private TMP_Text nameReward;

        [Header("Anim")]
        [SerializeField] private float animDuration = 0.2f;

        [SerializeField] private ButtonExtension btnClaim;

        private void Awake()
        {
            InitSingleton();
            
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            btnClaim.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            btnClaim.onClick.RemoveListener(Hide);
        }

        [Button]
        public void Show(string description, Sprite icon, string nameReward)
        {
            AudioAssistant.Shot(TypeSound.PopupReward);
            
            this.description.text = description;
            iconReward.sprite = icon;
            this.nameReward.text = nameReward;
            
            
            this.DOKill();
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;

            transform.DOScale(1f, animDuration).SetTarget(this);
        }

        
        void Hide()
        {
            this.DOKill();
            transform.DOScale(0f, animDuration).SetTarget(this).OnComplete(() => gameObject.SetActive(false));
            
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }
    }
}