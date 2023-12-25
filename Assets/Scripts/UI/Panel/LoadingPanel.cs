using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class LoadingPanel : UIPanel
    {
        [SerializeField] private Slider slider;
        
        [SerializeField] private TMP_Text textSlider;
        [SerializeField] private float loadingDuration = 1f;


        public override void Show(Action action = null)
        {
            base.Show(action);
            
            this.DOKill();
            
            SetValueSlider(0f);
            DOVirtual.Float(0f, 1f, loadingDuration, SetValueSlider).OnComplete(() =>
            {
                action?.Invoke();
            });
        }
        

        void SetValueSlider(float value)
        {
            slider.value = value;
            textSlider.text = (int)(value * 100) + "%";
        }
    }
}