using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class VictoryScreen : UIPanel
    {
        [SerializeField] private ButtonExtension nextLevelButton;
        [SerializeField] private ButtonExtension menuButton;

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            nextLevelButton.onClick.AddListener(NextLevel);
            menuButton.onClick.AddListener(Menu);
            
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            nextLevelButton.onClick.RemoveListener(NextLevel);
            menuButton.onClick.RemoveListener(Menu);
        }

        void NextLevel()
        {
            //TODO: next level logic
        }

        void Menu()
        {
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.MainScreen));
        }
    }
}