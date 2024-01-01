using System;
using CameraManager;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class UpgradePopup : UIPanel
    {
        [SerializeField] private ButtonExtension closeButton;

        public override void Show(Action action = null)
        {
            base.Show(action);
            
            CameraController.Instance.SetStatusCameraLeft(true);
        }

        public override void Hide(Action action = null)
        {
            base.Hide(action);
            
            CameraController.Instance.SetStatusCameraLeft(false);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            closeButton.onClick.AddListener(ClosePanel);
        }


        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            closeButton.onClick.RemoveListener(ClosePanel);
        }
        
        private void ClosePanel()
        {
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }

    }
}