using System;
using DefaultNamespace.Audio;
using DefaultNamespace.Configs;
using DefaultNamespace.Haptic;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.Daily_Reward
{
    public class DailyRewardItem : MonoBehaviour
    {
        public enum Status
        {
            NotClaimable,
            Claimable,
            Claimed
        }
    
        [SerializeField] private ButtonExtension button;
        [SerializeField] private GameObject coin, gift, claimed, claimable;
        [SerializeField] private TMP_Text coinVal;
        [SerializeField] private RewardType type;
        [SerializeField] private TMP_Text dayText;

        [Header("Gift")] [SerializeField] private Image iconGift;
        [SerializeField] private TMP_Text giftText;

        private Action _onClaim;
        private int _coinValue;
        private Status _status;
        private Action _onNotClaim;

        private void OnEnable()
        {
            button.onClick.AddListener(Claim);
            button.onClick.AddListener(() =>
            {
                if (_status == Status.NotClaimable)
                    _onNotClaim?.Invoke();
            });
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Claim);
            button.onClick.RemoveListener(() =>
            {
                if (_status == Status.NotClaimable)
                    _onNotClaim?.Invoke();
            });
        }

        public void SetDayText(int index)
        {
            this.dayText.text = "DAY " + index;
        }

        public void InitCoin(int value, Action onClaim, Action onNotClaim)
        {
            type = RewardType.Coin;
            _onClaim = onClaim;
            _onNotClaim = onNotClaim;
            
            _coinValue = value;

            UpdateVisual();
        }
    
        public void InitGift(Action onClaim, Action onNotClaim)
        {
            type = RewardType.Gift;
            _onClaim = onClaim;
            _onNotClaim = onNotClaim;
            // TODO: Init gift logic

            if (GameController.Instance.SetDailyReward())
            {
                var config =
                    ConfigManager.Instance.animMainGroupConfig.GetAnimMainConfig(GameManager.Instance.GameData.userData
                        .currentDailyReward);

                iconGift.sprite = config.icon;
                giftText.text = config.animName;
                UpdateVisual();
                
                return;
            }
            
            InitCoin(1000, onClaim, onNotClaim);
        }

        void UpdateVisual()
        {
            coin.SetActive(type == RewardType.Coin);
            gift.SetActive(type == RewardType.Gift);

            coinVal.text = _coinValue.ToFormatString();
        }
    
        public void SetStatus(Status status)
        {
            button.interactable = status != Status.Claimed;
            _status = status;
            claimable.SetActive(status == Status.Claimable);
            claimed.SetActive(status == Status.Claimed);
        }

        void Claim()
        {
            if (_status != Status.Claimable) return;
            
            AudioAssistant.Shot(TypeSound.DailyReward);
            
            BHHaptic.Haptic(HapticTypes.SoftImpact);
            
            switch (type)
            {
                case RewardType.Coin:
                    //GameManager.Instance.AddMoney(_coinValue);
                    EventGlobalManager.Instance.OnClaimMoney.Dispatch(_coinValue);
                        
                    SetStatus(Status.Claimed);
                    break;
                case RewardType.Gift:
                    // TODO: Claim gift logic
                    GameManager.Instance.GameData.userData.mainAnimItemStatusMap[
                        GameManager.Instance.GameData.userData.currentDailyReward] = true;
                    
                    var config =
                        ConfigManager.Instance.animMainGroupConfig.GetAnimMainConfig(GameManager.Instance.GameData.userData
                            .currentDailyReward);
                    
                    PopupReward.Instance.Show("Complete Login 7 days", config.icon, config.animName);
                    SetStatus(Status.Claimed);
                    break;
            }

            GameManager.Instance.GameData.userData.dailyRewardClaimedCount++;
            GameManager.Instance.GameData.userData.lastDailyRewardClaimTime = DateTime.Now;
            _onClaim?.Invoke();
        }
    }
}