using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class MainScreen : UIPanel
    {
        [Header("Start Game")] 
        [SerializeField] private ButtonExtension startGameButton;
        
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            startGameButton.onClick.AddListener(StartGame);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            startGameButton.onClick.RemoveListener(StartGame);
        }

        void StartGame()
        {
            GameController.Instance.StartGame();
            
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.PlayScreen));
        }
    }
}