using System;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        public Canvas root;
        public PanelAnim panelAnim;

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
            gameObject.SetActive(true);
            if (panelAnim) 
                panelAnim.StartAnimIn();
        }

        public virtual void Hide(Action action = null)
        {
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