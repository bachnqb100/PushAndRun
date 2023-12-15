using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    [RequireComponent(typeof(ButtonAnim))]
    public class ButtonExtension : Button
    {
        public ButtonAnim buttonAnim;

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
    }
}