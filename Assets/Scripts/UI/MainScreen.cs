using System;
using CameraManager;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class MainScreen : UIPanel
    {
        [Header("Start Game")] 
        [SerializeField] private ButtonExtension startGameButton;

        [Header("Button Component")] 
        [SerializeField] private Button settingButton;

        public override void Show(Action action = null)
        {
            base.Show(action);
            
            GameController.Instance.SetGameStatus(GameStatus.MainScreen);
            
            CameraController.Instance.SetStatusCameraMain(true);
        }

        protected override void Disable()
        {
            base.Disable();
            
            CameraController.Instance.SetStatusCameraMain(false);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            startGameButton.onClick.AddListener(StartGame);
            settingButton.onClick.AddListener(OpenSetting);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            startGameButton.onClick.RemoveListener(StartGame);
            settingButton.onClick.RemoveListener(OpenSetting);
        }

        void StartGame()
        {
            GameController.Instance.StartGame();
            
            GUIManager.Instance.ShowPanel(PanelType.Loading, () => GUIManager.Instance.ShowPanel(PanelType.PlayScreen));
        }

        void OpenSetting()
        {
            GUIManager.Instance.ShowPanel(PanelType.Setting);
        }
    }
}