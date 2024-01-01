using DefaultNamespace.Audio;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.LuckyWheel
{
    public class WheelItem : MonoBehaviour
    {
        [SerializeField] private GameObject coin;
        [SerializeField] private GameObject gift;
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private RewardType type;
    
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
        
            coin.SetActive(false);
            gift.SetActive(true);
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
                
                    break;
            }
        }
    }
}