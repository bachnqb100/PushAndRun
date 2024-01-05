using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace DefaultNamespace.UI
{
    public class CutoutMask : MonoBehaviour
    {
        [SerializeField] private RectTransform cutoutMaskRect;
        [SerializeField] private float cutMaskDuration = 1f;
        [SerializeField] private float widthScreen = 3000f;
        
        
        [Button]
        public void CutOutMask(Action OnComplete = null)
        {
            var sizeDelta = cutoutMaskRect.sizeDelta;
            DOVirtual.Float(0f, widthScreen, cutMaskDuration, x =>
            {
                sizeDelta.x = x;
                cutoutMaskRect.sizeDelta = sizeDelta;
            }).OnComplete(() => OnComplete?.Invoke());
        }
        
        [Button]
        public void CutInMask(Action OnComplete = null)
        {
            var sizeDelta = cutoutMaskRect.sizeDelta;
            DOVirtual.Float(widthScreen, 0f, cutMaskDuration, x =>
            {
                sizeDelta.x = x;
                cutoutMaskRect.sizeDelta = sizeDelta;
            }).OnComplete(() => OnComplete?.Invoke());
        }

        [Button]
        public void ResetCutoutMask()
        {
            #if UNITY_EDITOR
            var sizeDelta = new Vector2(1920f, 1080f);
            cutoutMaskRect.sizeDelta = sizeDelta;
            #else
            var sizeDelta = new Vector2(Screen.width, Screen.height);
            cutoutMaskRect.sizeDelta = sizeDelta;
            #endif
        }
    }
}