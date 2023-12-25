using System;
using CameraManager;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ClothesPanel : UIPanel
    {
        [SerializeField] private Button closeButton;

        public override void Show(Action action = null)
        {
            base.Show(action);
            
            CameraController.Instance.SetStatusCameraClothes(true);
        }

        public override void Hide(Action action = null)
        {
            base.Hide(action);
            
            CameraController.Instance.SetStatusCameraClothes(false);
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
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }
    }
}