using System;
using CameraManager;
using DefaultNamespace.Haptic;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ClothesPanel : UIPanel
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
            
            closeButton.onClick.AddListener(CloseClothesPanel);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            closeButton.onClick.RemoveListener(CloseClothesPanel);
        }


        void CloseClothesPanel()
        {
            BHHaptic.Haptic(HapticTypes.Selection);
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }
    }
}