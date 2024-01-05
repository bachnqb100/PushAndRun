using System;
using DefaultNamespace.Audio;
using DefaultNamespace.Haptic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
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

        [Title("Money")] 
        [SerializeField] private TMP_Text moneyPunch;
        [SerializeField] private TMP_Text moneyTime;
        [SerializeField] private TMP_Text moneyTotal;
        


        private int _totalMoney;
        
        public override void Show(Action action = null)
        {
            base.Show(action);
            
            AnimShow();
            HideButton();
            InitItemComplete();
            SetupMoneyText();
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
            BHHaptic.Haptic(HapticTypes.LightImpact);
            
            //TODO: next level logic
            GameController.Instance.StartGame();
            
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.PlayScreen));
        }

        void Menu()
        {
            BHHaptic.Haptic(HapticTypes.SoftImpact);
            
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
            }).SetTarget(this);
        }

        void SetupMoneyText()
        {
            _totalMoney = GameController.Instance.CalculateMoneyVictory(out var moneyTime, out var moneyPunch);

            DOVirtual
                .Float(0f, moneyTime, animChangeItemCompleteDuration,
                    x => { this.moneyTime.text = ((int)x).ToString(); }).OnStart(() =>
                {
                    DOVirtual.Float(0f, moneyPunch, animChangeItemCompleteDuration,
                        x => { this.moneyPunch.text = ((int)x).ToString(); }).SetTarget(this);
                })
                .OnComplete(() =>
                {
                    DOVirtual.Float(0f, _totalMoney, animChangeItemCompleteDuration,
                        x => { this.moneyTotal.text = ((int)x).ToString(); }).SetTarget(this).OnComplete(() =>
                    {
                        AudioAssistant.Shot(TypeSound.Reward);
                        EventGlobalManager.Instance.OnClaimMoney.Dispatch(_totalMoney);
                        ShowButton();
                    });
                }).SetTarget(this);

        }
        
    }
}