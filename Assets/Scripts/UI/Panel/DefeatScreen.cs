using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class DefeatScreen : UIPanel
    {
        [Title("Effect")] 
        [SerializeField] private CutoutMask cutOutMask;
        
        [Header("Defeat text")]
        [SerializeField] private float showDefeatTextDuration = 1f;
        [SerializeField] private float defaultScale = 0.5f;
        [SerializeField] private float defaultAlpha = 0.5f;
        [SerializeField] private float alphaDuration = 2f;
        [SerializeField] private CanvasGroup canvasGroupDefeatText;

        [Header("Defeat reason")] 
        [SerializeField] private float showDefeatReasonAlphaDuration = 1f;

        [SerializeField] private CanvasGroup canvasGroupDefeatReason;
            

        [Header("Defeat reason")] 
        [SerializeField] private TMP_Text defeatText;
        [SerializeField] private TMP_Text defeatReasonText;
        
        [Title("Button")]
        [SerializeField] private ButtonExtension menuButton;
        [SerializeField] private ButtonExtension retryButton;

        public override void Show(Action action = null)
        {
            base.Show(action);
            
            HideAllItem();
            
            cutOutMask.CutInMask(ShowItem);
            
            switch (GameController.Instance.DefeatReason)
            {
                case DefeatReason.Fall:
                    defeatReasonText.text = "fall down";
                    break;
                case DefeatReason.Timeout:
                    defeatReasonText.text = "timeout";
                    break;
                case DefeatReason.Detect:
                    defeatReasonText.text = "Detect";
                    break;
            }
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            menuButton.onClick.AddListener(Menu);
            retryButton.onClick.AddListener(Retry);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            menuButton.onClick.RemoveListener(Menu);
            retryButton.onClick.RemoveListener(Retry);
        }

        void Retry()
        {
            //TODO: Logic retry
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.PlayScreen));
            
            GameController.Instance.Player.ResetPlayer();
            GameController.Instance.StartGame();
        }

        void Menu()
        {
            GameController.Instance.PlacePlayerMain();
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.MainScreen));
        }

        [Button]
        void ShowItem()
        {
            defeatText.transform.localScale = Vector3.one * defaultScale;
            defeatText.transform.DOScale(1f, showDefeatTextDuration).OnStart(() =>
            {
                canvasGroupDefeatText.alpha = defaultAlpha;
                canvasGroupDefeatText.DOFade(1f, alphaDuration);
            }).OnComplete(() =>
            {
                canvasGroupDefeatReason.DOFade(1f, showDefeatReasonAlphaDuration).OnComplete(() =>
                {
                    //Show button
                    menuButton.Show();
                    retryButton.Show();
                });
            });
        }

        void HideAllItem()
        {
            canvasGroupDefeatText.alpha = 0f;
            canvasGroupDefeatReason.alpha = 0f;
            
            menuButton.Hide();
            retryButton.Hide();
        }
    }
}