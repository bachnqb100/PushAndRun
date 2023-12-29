using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.Online_Reward
{
    public class OnlineRewardButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text timer;
        [SerializeField] private GameObject notif;
        [SerializeField] private ButtonExtension btn;

        private void OnEnable()
        {
            EventGlobalManager.Instance.OnEverySecondTick.AddListener(UpdateTimer);
            btn.onClick.AddListener(OpenPopup);
            UpdateTimer();
        }

        private void OnDisable()
        {
            if (EventGlobalManager.Instance)
            {
                EventGlobalManager.Instance.OnEverySecondTick.AddListener(UpdateTimer);
                btn.onClick.RemoveListener(OpenPopup);
            }
        }
    
        void UpdateTimer()
        {
            int timeRemain = GameManager.GetRemainTime();

            if (timeRemain < 0)
            {
                notif.SetActive(true);
                timer.text = "Claim";
            }
            else
            {
                notif.SetActive(false);
                timer.text = timeRemain.ToTimeFormatCompact();
            }
        }

        public void OpenPopup()
        {
            GUIManager.Instance.ShowPanel(PanelType.OnlineReward);
        }
    }
}