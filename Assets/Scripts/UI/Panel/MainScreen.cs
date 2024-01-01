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
        [SerializeField] private ButtonExtension settingButton;
        [SerializeField] private ButtonExtension clothesButton;
        [SerializeField] private ButtonExtension upgradeButton;
        [SerializeField] private ButtonExtension shopButton;

        public override void Show(Action action = null)
        {
            base.Show(action);
            
            GameController.Instance.SetGameStatus(GameStatus.MainScreen);
            
            CameraController.Instance.SetStatusCameraMain(true);
        }

        protected override void Disable()
        {
            base.Disable();
            
            if (CameraController.Instance)
                CameraController.Instance.SetStatusCameraMain(false);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            startGameButton.onClick.AddListener(StartGame);
            settingButton.onClick.AddListener(OpenSetting);
            clothesButton.onClick.AddListener(OpenClothes);
            upgradeButton.onClick.AddListener(OpenUpgrade);
            shopButton.onClick.AddListener(OpenShop);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            startGameButton.onClick.RemoveListener(StartGame);
            settingButton.onClick.RemoveListener(OpenSetting);
            clothesButton.onClick.RemoveListener(OpenClothes);
            upgradeButton.onClick.RemoveListener(OpenUpgrade);
            shopButton.onClick.RemoveListener(OpenShop);

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

        void OpenClothes()
        {
            GUIManager.Instance.ShowPanel(PanelType.Clothes);
        }
        void OpenUpgrade()
        {
            GUIManager.Instance.ShowPanel(PanelType.Upgrade);
        }

        void OpenShop()
        {
            GUIManager.Instance.ShowPanel(PanelType.Shop);
        }
    }
}