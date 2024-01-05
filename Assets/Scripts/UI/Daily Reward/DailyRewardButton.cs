using DefaultNamespace.Haptic;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.Daily_Reward
{
    public class DailyRewardButton : MonoBehaviour
    {
        [SerializeField] private GameObject notif;
        [SerializeField] private ButtonExtension btn;

        private void OnEnable()
        {
            btn.onClick.AddListener(OpenPopup);
            UpdateNotif();
        }

        private void OnDisable()
        {
            if (EventGlobalManager.Instance)
            {
                btn.onClick.RemoveListener(OpenPopup);
            }
        }
    
        void UpdateNotif()
        {
            notif.SetActive(GameManager.IsClaimableDailyReward());
        }

        void OpenPopup()
        {
            BHHaptic.Haptic(HapticTypes.Selection);
            
            GUIManager.Instance.ShowPanel(PanelType.DailyReward);
        }
    }
}