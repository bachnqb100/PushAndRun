using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class VictoryScreen : UIPanel
    {
        [SerializeField] private ButtonExtension nextLevelButton;
        [SerializeField] private ButtonExtension menuButton;

        [Title("Anim Bg Show")] 
        [SerializeField] private CanvasGroup canvasGroupAnim;
        [SerializeField] private float alphaValue = 0.2f;
        [SerializeField] private float animDuration = 1f;

        [Title("Item Complete")] 
        [SerializeField] private TMP_Text impactEnemyText;
        [SerializeField] private TMP_Text timeCompletedText;
        [SerializeField] private float animChangeItemCompleteDuration = 2f;
        

        public override void Show(Action action = null)
        {
            base.Show(action);
            
            AnimShow();
            HideButton();
            InitItemComplete();
        }

        [Button]
        void AnimShow()
        {
            canvasGroupAnim.alpha = 0f;
            DOVirtual.Float(0f, alphaValue, animDuration, x =>
            {
                canvasGroupAnim.alpha = x;
            }).SetTarget(this);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            nextLevelButton.onClick.AddListener(NextLevel);
            menuButton.onClick.AddListener(Menu);
            
            panelAnim.animInCompletedEvent.AddListener(ShowItemComplete);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            nextLevelButton.onClick.RemoveListener(NextLevel);
            menuButton.onClick.RemoveListener(Menu);
            
            panelAnim.animInCompletedEvent.RemoveListener(ShowItemComplete);
        }

        void NextLevel()
        {
            //TODO: next level logic
        }

        void Menu()
        {
            GameController.Instance.PlacePlayerMain();
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.MainScreen));
        }

        void HideButton()
        {
            menuButton.Hide();
            nextLevelButton.Hide();
        }

        void ShowButton()
        {
            menuButton.Show();
            nextLevelButton.Show();
        }

        void InitItemComplete()
        {
            impactEnemyText.text = "never tried it";
            timeCompletedText.text = 0.ToTimeFormatCompact();
        }

        void ShowItemComplete()
        {
            if (GameController.Instance.ImpactCount == 0)
            {
                
            }
            else if (GameController.Instance.ImpactCount >= 1)
            {
                DOVirtual.Float(0f, GameController.Instance.ImpactCount, animChangeItemCompleteDuration, x =>
                {
                    impactEnemyText.text = ((int)x) + " times";
                }).SetTarget(this);
            }

            DOVirtual.Float(0f, GameController.Instance.TimeCompleted, animChangeItemCompleteDuration, x =>
            {
                timeCompletedText.text = ((int)x).ToTimeFormatCompact();
            }).OnComplete(ShowButton).SetTarget(this);
        }
        
    }
}