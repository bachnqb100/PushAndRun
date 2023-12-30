using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.LuckyWheel
{
    public class LuckyWheelProgress : MonoBehaviour
    {
        [SerializeField] private Slider progressSlider;
        [SerializeField] private TMP_Text progressTxt;
        [SerializeField] private SerializedDictionary<int, LuckyWheelProgressReward> rewardMap;

        public void Init()
        {
            var progress = GameManager.Instance.GameData.userData.luckyWheelProgress;
            progressSlider.value = progress;

            foreach (var pair in rewardMap)
            {
                pair.Value.SetClaimed(pair.Key <= progress);
            }
        }

        public void UpdateProgress()
        {
            var progress = GameManager.Instance.GameData.userData.luckyWheelProgress;
            progressTxt.text = $"Progress: {progress}/{progressSlider.maxValue}";
            progressSlider.DOValue(progress, .5f).OnComplete(() =>
            {
                foreach (var pair in rewardMap)
                {
                    if (pair.Key == progress)
                        pair.Value.Claim();
                    else
                        pair.Value.SetClaimed(pair.Key <= progress);
                }
            });
        }

        public void ClaimReward1()
        {
            // TODO: Claim reward logic
            EventGlobalManager.Instance.OnClaimMoney.Dispatch(300);
        }
    
        public void ClaimReward2()
        {
            // TODO: Claim reward logic
            EventGlobalManager.Instance.OnClaimMoney.Dispatch(500);
        }
    
        public void ClaimReward3()
        {
            // TODO: Claim reward logic
            EventGlobalManager.Instance.OnClaimMoney.Dispatch(1000);
        }
    }
}