using DefaultNamespace.Configs;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class OnlineRewardPopup : UIPanel
    {
        [SerializeField] private Transform claimableRoot;
        [SerializeField] private Transform notClaimableRoot;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text rewardValueTxt;

        [SerializeField] private ButtonExtension btnClaim, btnClaimAds, btnContinue;
        
        private int _rewardValue;
        private bool _claimable;

        protected override void Init()
        {
            
            base.Init();
            #region Init reward

            _rewardValue = ConfigManager.Instance.extraFeaturesConfig.onlineRewardValue;
            rewardValueTxt.text = _rewardValue.ToFormatString();

            #endregion
            
            _claimable = GameManager.GetRemainTime() < 0;
            
            if (_claimable)
                ShowGift(true);
            else
                HideGift(true);
            
            UpdateTimer();
        }
        
        void ShowGift(bool isInit = false)
        {
            _claimable = true;
            
            if (isInit)
                ShowClaimable();
            else
            {
                notClaimableRoot.gameObject.SetActive(true);
                claimableRoot.gameObject.SetActive(false);
                notClaimableRoot.localScale = Vector3.one;
                notClaimableRoot.DOScale(0, .3f).OnComplete(ShowClaimable);
            }

            void ShowClaimable()
            {
                notClaimableRoot.gameObject.SetActive(false);
                claimableRoot.gameObject.SetActive(true);
                claimableRoot.localScale = Vector3.zero;
                claimableRoot.DOScale(1, .3f);
            }
        }

        void HideGift(bool isInit = false)
        {
            _claimable = false;

            if (isInit)
                ShowNotClaimable();
            else
            {
                claimableRoot.gameObject.SetActive(true);
                notClaimableRoot.gameObject.SetActive(false);
                claimableRoot.localScale = Vector3.one;
                claimableRoot.DOScale(0, .3f).OnComplete(ShowNotClaimable);
            }

            void ShowNotClaimable()
            {
                claimableRoot.gameObject.SetActive(false);
                notClaimableRoot.gameObject.SetActive(true);
                notClaimableRoot.localScale = Vector3.zero;
                notClaimableRoot.DOScale(1, .3f);
            }
        }
        
        void UpdateTimer()
        {
            if (_claimable)
                return;

            int timeRemain = GameManager.GetRemainTime();

            if (timeRemain < 0)
                ShowGift();
            else
                timer.text = timeRemain.ToTimeFormatCompact();
        }

        public void Claim()
        {
            #region Claim logic

            GameManager.Instance.AddMoney(_rewardValue);

            #endregion
            
            ResetGiftTimer();
            Close();
        }

        public void ClaimAds()
        {
            #region Claim ads logic

                GameManager.Instance.AddMoney(_rewardValue * 3);

                #endregion

                ResetGiftTimer();
                Close();
        }
        
        void ResetGiftTimer()
        {
            GameManager.Instance.LastClaimOnlineGiftTime = Time.time;
        }
        
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            btnClaim.onClick.AddListener(Claim);
            btnClaimAds.onClick.AddListener(ClaimAds);
            btnContinue.onClick.AddListener(ClosePopup);
            
            EventGlobalManager.Instance.OnEverySecondTick.AddListener(UpdateTimer);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();

            if (EventGlobalManager.Instance)
            {
                btnClaim.onClick.RemoveListener(Claim);
                btnClaimAds.onClick.RemoveListener(ClaimAds);
                btnContinue.onClick.RemoveListener(ClosePopup);
                
                EventGlobalManager.Instance.OnEverySecondTick.RemoveListener(UpdateTimer);
            }
        }

        void ClosePopup()
        {
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }
    }
}