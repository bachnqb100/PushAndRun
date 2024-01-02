using DefaultNamespace.Audio;
using DefaultNamespace.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.LuckyWheel
{
    public class WheelItem : MonoBehaviour
    {
        [SerializeField] private GameObject coin;
        [SerializeField] private GameObject gift;
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private RewardType type;

        [Header("Gift")] [SerializeField] private Image iconGift;
        [SerializeField] private TMP_Text giftText;
    
        private int _value;

        public void InitCoin(int val)
        {
            type = RewardType.Coin;
            _value = val;
            coin.SetActive(true);
            gift.SetActive(false);
            valueText.text = val.ToFormatString();
        }

        public void InitGift()
        {
            type = RewardType.Gift;
        
            // TODO: Init gift reward

            if (GameController.Instance.SetWheelReward())
            {
                var config =
                    ConfigManager.Instance.animVictoryGroupConfig.GetAnimVictoryConfig(GameManager.Instance.GameData.userData
                        .currentWheelReward);

                iconGift.sprite = config.icon;
                giftText.text = config.animName;
                
                coin.SetActive(false);
                gift.SetActive(true);
                
                return;
            }
            
            InitCoin(1000);
            
        }

        public void Claim()
        {
            AudioAssistant.Shot(TypeSound.RewardSpin);
            
            switch (type)
            {
                case RewardType.Coin:
                    EventGlobalManager.Instance.OnClaimMoney.Dispatch(_value);
                    break;
                case RewardType.Gift:
                    // TODO: Claim gift logic
                    
                    GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[
                        GameManager.Instance.GameData.userData.currentWheelReward] = true;
                    
                    var config =
                        ConfigManager.Instance.animVictoryGroupConfig.GetAnimVictoryConfig(GameManager.Instance.GameData.userData
                            .currentWheelReward);
                    
                    PopupReward.Instance.Show("Wheel special reward", config.icon, config.animName);
                
                    break;
            }
        }
    }
}