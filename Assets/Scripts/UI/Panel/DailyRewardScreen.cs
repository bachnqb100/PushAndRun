using System;
using DefaultNamespace.Configs;
using DefaultNamespace.UI.Daily_Reward;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class DailyRewardScreen : UIPanel
    {
        [SerializeField] private DailyRewardItem[] dailyRewardItems;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private Button closeButton;
        
        [Header("Skip time")]
        [SerializeField] private ButtonExtension skipButton;
        [SerializeField] private GameObject skipTimeContainer;
        [SerializeField] private Button closeSkipTimeButton;
        [SerializeField] private Notify notify;

        protected override void Init()
        {
            base.Init();
            
            for (var i = 0; i < dailyRewardItems.Length; i++)
            {
                dailyRewardItems[i].SetDayText(i + 1);
                
                if (i < dailyRewardItems.Length - 1)
                    dailyRewardItems[i].InitCoin(ConfigManager.Instance.extraFeaturesConfig.dailyRewardCoinRewardValues[i], UpdateTimer, OpenSkipTimePanel);
                else
                    dailyRewardItems[i].InitGift(UpdateTimer, OpenSkipTimePanel);
            }
            
            UpdateTimer();
            CloseSkipTimePanel();
            notify.Hide();
        }

        void UpdateTimer()
        {
            int currentDayIndex = GameManager.Instance.GameData.userData.dailyRewardClaimedCount % 7;
            var isClaimable = IsClaimable();

            if (!isClaimable)
            {
                timer.text = GameManager.GetRemainTimeDailyReward().ToTimeFormatCompact();
            }
            
            for (var i = 0; i < dailyRewardItems.Length; i++)
            {
                if (i < currentDayIndex)
                    dailyRewardItems[i].SetStatus(DailyRewardItem.Status.Claimed);
                else if (i == currentDayIndex)
                    dailyRewardItems[i].SetStatus(isClaimable 
                        ? DailyRewardItem.Status.Claimable 
                        : DailyRewardItem.Status.NotClaimable);
                else
                    dailyRewardItems[i].SetStatus(DailyRewardItem.Status.NotClaimable);
            }
        }

        public static bool IsClaimable()
        {
            var now = DateTime.Now;
            bool isClaimable = now.Day > GameManager.Instance.GameData.userData.lastDailyRewardClaimTime.Day ||
                               now.Month > GameManager.Instance.GameData.userData.lastDailyRewardClaimTime.Month ||
                               now.Year > GameManager.Instance.GameData.userData.lastDailyRewardClaimTime.Year;
            return isClaimable;
        }

        void SkipDay()
        {
            GameManager.Instance.GameData.userData.lastDailyRewardClaimTime = DateTime.MinValue;
            UpdateTimer();
        }
        

        
        protected override void RegisterEvent()
        {
            base.RegisterEvent();

            closeButton.onClick.AddListener(ClosePanel);
            skipButton.onClick.AddListener(SkipDay);
            skipButton.onClick.AddListener(CloseSkipTimePanel);
            closeSkipTimeButton.onClick.AddListener(CloseSkipTimePanel);
            
            EventGlobalManager.Instance.OnEverySecondTick.AddListener(UpdateTimer);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();

            if (EventGlobalManager.Instance)
            {
                closeButton.onClick.RemoveListener(ClosePanel);
                skipButton.onClick.RemoveListener(SkipDay);
                skipButton.onClick.RemoveListener(CloseSkipTimePanel);
                closeSkipTimeButton.onClick.RemoveListener(CloseSkipTimePanel);
                
                EventGlobalManager.Instance.OnEverySecondTick.RemoveListener(UpdateTimer);
            }
        }

        void ClosePanel()
        {
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }

        void OpenSkipTimePanel()
        {
            if (IsClaimable())
            {
                notify.Show();
                return;
            }
            skipTimeContainer.SetActive(true);
        }
        
        void CloseSkipTimePanel()
        {
            skipTimeContainer.SetActive(false);
        }
    }
}