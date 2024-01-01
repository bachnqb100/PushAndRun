using System;
using DefaultNamespace.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        [Title("Root")]
        public Canvas root;
        public PanelAnim panelAnim;

        [Title("Sound")]
        public string musicName;

        public TypeSound openPanel = TypeSound.OpenPanel;
        public TypeSound closePanel = TypeSound.ClosePanel;


        
        

        private bool _init;

        public void OnEnable()
        {
            Init();
            RegisterEvent();
        }

        public void OnDisable()
        {
            UnregisterEvent();
            Disable();
        }

        public virtual void Show(Action action = null)
        {
            AudioAssistant.Instance.PlayMusic(musicName);
            gameObject.SetActive(true);
            
            AudioAssistant.Shot(openPanel);
            
            if (panelAnim) 
                panelAnim.StartAnimIn();
        }

        public virtual void Hide(Action action = null)
        {
            AudioAssistant.Shot(closePanel);
            
            if (panelAnim)
                panelAnim.StartAnimOut();
            else 
                Close();
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }

        protected virtual void Init()
        {
            if (_init) return;
            _init = true;
            
            if (panelAnim)
                panelAnim.Setup(this);
        }

        protected virtual void Disable()
        {
            
        }
        
        protected virtual void RegisterEvent()
        {
            
        }

        protected virtual void UnregisterEvent()
        {
            
        }


    }
}