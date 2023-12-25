using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class SettingPopup : UIPanel
    {
        [SerializeField] private ButtonExtension closeButton;
        
        [Header("Music")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private GameObject musicEnable;
        [SerializeField] private GameObject musicDisable;
        
        [Header("SFX")]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private GameObject sfxEnable;
        [SerializeField] private GameObject sfxDisable;

        [Header("Haptic")] 
        [SerializeField] private Button hapticBtn;
        [SerializeField] private GameObject hapticEnable, hapticDisable, hapticEnableIcon, hapticDisableIcon;
        
        
        public override void Show(Action action = null)
        {
            base.Show(action);
            
            Setup();
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
            sfxSlider.onValueChanged.AddListener(UpdateSfxVolume);
            hapticBtn.onClick.AddListener(UpdateHaptic);
            
            closeButton.onClick.AddListener(ClosePopup);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            musicSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
            sfxSlider.onValueChanged.RemoveListener(UpdateSfxVolume);
            hapticBtn.onClick.RemoveListener(UpdateHaptic);
            
            closeButton.onClick.RemoveListener(ClosePopup);
        }
        

        void Setup()
        {
            musicSlider.value = GameManager.Instance.GameData.setting.bgMusicVolume;
            musicEnable.SetActive(GameManager.Instance.GameData.setting.bgMusicVolume > 0);
            musicDisable.SetActive(!(GameManager.Instance.GameData.setting.bgMusicVolume > 0));
            
            sfxSlider.value = GameManager.Instance.GameData.setting.sfxVolume;
            sfxEnable.SetActive(GameManager.Instance.GameData.setting.sfxVolume > 0);
            sfxDisable.SetActive(!(GameManager.Instance.GameData.setting.sfxVolume > 0));
            
            hapticEnable.SetActive(GameManager.Instance.GameData.setting.haptic);
            hapticDisable.SetActive(!GameManager.Instance.GameData.setting.haptic);
            hapticEnableIcon.SetActive(GameManager.Instance.GameData.setting.haptic);
            hapticDisableIcon.SetActive(!GameManager.Instance.GameData.setting.haptic);
        }

        void UpdateMusicVolume(float volume)
        {
            GameManager.Instance.GameData.setting.bgMusicVolume = volume;
            musicEnable.SetActive(volume > 0);
            musicDisable.SetActive(!(volume > 0));
            
            EventGlobalManager.Instance.OnUpdateSetting.Dispatch();
        }

        void UpdateSfxVolume(float volume)
        {
            GameManager.Instance.GameData.setting.sfxVolume = volume;
            sfxEnable.SetActive(volume > 0);
            sfxDisable.SetActive(!(volume > 0));
            
            EventGlobalManager.Instance.OnUpdateSetting.Dispatch();
        }

        void UpdateHaptic()
        {
            GameManager.Instance.GameData.setting.haptic = !GameManager.Instance.GameData.setting.haptic;
            hapticEnable.SetActive(GameManager.Instance.GameData.setting.haptic);
            hapticDisable.SetActive(!GameManager.Instance.GameData.setting.haptic);
            hapticEnableIcon.SetActive(GameManager.Instance.GameData.setting.haptic);
            hapticDisableIcon.SetActive(!GameManager.Instance.GameData.setting.haptic);
        }

        void ClosePopup()
        {
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }
    }
}