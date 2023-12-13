using System;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        public void OnEnable()
        {
            RegisterEvent();
            Init();
        }

        public void OnDisable()
        {
            UnregisterEvent();
            Disable();
        }

        public virtual void Show(Action action = null)
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide(Action action = null)
        {
            gameObject.SetActive(false);
        }

        protected virtual void Init()
        {
            
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