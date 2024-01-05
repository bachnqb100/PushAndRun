using System;
using DefaultNamespace.Haptic;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace DefaultNamespace.UI.Shop
{
    public class TabShopButton : MonoBehaviour
    {
        [SerializeField] private ButtonExtension btn;
        [SerializeField] private GameObject selected, deSelect;
        [SerializeField] private GameObject content;
        [SerializeField] private GameStatus status;
        private Action _onClickButton;


        private void OnEnable()
        {
            btn.onClick.AddListener(ShowContent);
        }

        private void OnDisable()
        {
            btn.onClick.RemoveListener(ShowContent);
        }

        public void Init(Action onClickButton)
        {
            _onClickButton = onClickButton;
            
            HideContent();
        } 
        
        public void ShowContent()
        {
            _onClickButton?.Invoke();
            
            BHHaptic.Haptic(HapticTypes.Selection);

            
            content.SetActive(true);
            selected.SetActive(true);
            deSelect.SetActive(false);

            btn.interactable = false;
            GameController.Instance.SetGameStatus(status);
        }

        public void HideContent()
        {
            content.SetActive(false);
            selected.SetActive(false);
            deSelect.SetActive(true);
            
            btn.interactable = true;
        }
    }
}