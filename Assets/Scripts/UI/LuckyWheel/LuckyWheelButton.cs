using DefaultNamespace.Haptic;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.LuckyWheel
{
    public class LuckyWheelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text timer;
        [SerializeField] private GameObject notif;
        [SerializeField] private ButtonExtension btn;

        private void OnEnable()
        {
            btn.onClick.AddListener(OpenPopup);
            
            EventGlobalManager.Instance.OnEverySecondTick.AddListener(UpdateTimer);
            UpdateTimer();
        }

        private void OnDisable()
        {
            btn.onClick.RemoveListener(OpenPopup);
            
            if (EventGlobalManager.Instance)
                EventGlobalManager.Instance.OnEverySecondTick.AddListener(UpdateTimer);
        }

        void UpdateTimer()
        {
            // Get remain time
            int timeRemain = GameManager.GetRemainTimeWheel();

            if (timeRemain < 0)
            {
                notif.SetActive(true);
                timer.text = "Spin";
            }
            else
            {
                notif.SetActive(false);
                timer.text = timeRemain.ToTimeFormatCompact();
            }
        }

        public void OpenPopup()
        {
            BHHaptic.Haptic(HapticTypes.Selection);
            
            GUIManager.Instance.ShowPanel(PanelType.LuckyWheel);
        }
    }
}