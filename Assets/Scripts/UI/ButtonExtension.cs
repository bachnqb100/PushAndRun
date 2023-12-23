using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    [RequireComponent(typeof(ButtonAnim))]
    public class ButtonExtension : Button
    {
        public ButtonAnim buttonAnim;

        public UnityEvent OnPointerDownEvent = new UnityEvent();
        public UnityEvent OnPointerUpEvent = new UnityEvent();

        protected override void Awake()
        {
            base.Awake();
            
            if (buttonAnim == null)
                Setup();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Show();
        }

        public void Show(bool instant = false)
        {
            gameObject.SetActive(true);

            if (!instant && buttonAnim)
                buttonAnim.ShowAnim();
        }

        public void Hide(bool instant = false)
        {
            if (instant || !buttonAnim)
                gameObject.SetActive(false);
            else
                buttonAnim.HideAnim(() => gameObject.SetActive(false));
        }
        

        void Setup()
        {
            buttonAnim = GetComponent<ButtonAnim>();
        }

        protected override void Reset()
        {
            base.Reset();
            Setup();
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            OnPointerDownEvent?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            OnPointerUpEvent?.Invoke();
        }
    }
}