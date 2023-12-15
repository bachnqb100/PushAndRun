using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class DefeatScreen : UIPanel
    {
        [SerializeField] private ButtonExtension menuButton;
        [SerializeField] private ButtonExtension retryButton;


        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            menuButton.onClick.AddListener(Menu);
            retryButton.onClick.AddListener(Retry);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            menuButton.onClick.RemoveListener(Menu);
            retryButton.onClick.RemoveListener(Retry);
        }

        void Retry()
        {
            //TODO: Logic retry
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.PlayScreen));
        }

        void Menu()
        {
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.MainScreen));
        }
    }
}