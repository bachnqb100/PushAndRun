using System;
using DefaultNamespace.Audio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ButtonAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Enums

        public enum ShowAnimType
        {
            None,
            FadeIn,
            FromScale
        }

        public enum PressAnimType
        {
            None,
            Scale
        }

        public enum HideAnimType
        {
            None,
            FadeOut,
            ToScale
        }

        #endregion
        
        #region Show Properties

        #region Show Anim
        
        private bool ShowButtonAnim => transform != null;
        
        private bool ShowShowAnimEasingType
        {
            get
            {
                if (showAnimType == ShowAnimType.FromScale && separateAxisShowAnim)
                    return false;

                return showAnimType != ShowAnimType.None;
            }
        }

        private bool ShowSeparateShowAnimEasingType => showAnimType == ShowAnimType.FromScale && separateAxisShowAnim;
        
        private bool UseShowAnimCurve
        {
            get
            {
                if (ShowSeparateShowAnimEasingType)
                    return false;
                
                return showAnimType != ShowAnimType.None && showAnimEasingType == EasingType.Custom;
            }
        }

        private bool UseXAxisShowAnimCurve => ShowSeparateShowAnimEasingType && showAnimXAxisEasingType == EasingType.Custom;
        
        private bool UseYAxisShowAnimCurve => ShowSeparateShowAnimEasingType && showAnimYAxisEasingType == EasingType.Custom;
        
        #endregion

        #region Press Anim

        private bool UsePressAnimCurve => pressAnimType != PressAnimType.None && pressAnimEasingType == EasingType.Custom;

        private bool UseReleaseAnimCurve => pressAnimType != PressAnimType.None && releaseAnimEasingType == EasingType.Custom;

        #endregion
        
        #region Hide Anim
        
        private bool ShowHideAnimEasingType
        {
            get
            {
                if (hideAnimType == HideAnimType.ToScale && separateAxisHideAnim)
                    return false;

                return hideAnimType != HideAnimType.None;
            }
        }
        
        private bool ShowSeparateHideAnimEasingType => hideAnimType == HideAnimType.ToScale && separateAxisHideAnim;

        private bool UseHideAnimCurve
        {
            get
            {
                if (ShowSeparateHideAnimEasingType)
                    return false;
                
                return hideAnimType != HideAnimType.None && hideAnimEasingType == EasingType.Custom;
            }
        }
        
        private bool UseXAxisHideAnimCurve => ShowSeparateHideAnimEasingType && hideAnimXAxisEasingType == EasingType.Custom;
        
        private bool UseYAxisHideAnimCurve => ShowSeparateHideAnimEasingType && hideAnimYAxisEasingType == EasingType.Custom;
        
        #endregion
        
        #endregion
        

        [SerializeField] private bool unscaleTime;
        
        #region Button Anim Fields
        
        #region Show Anim
        
        [Space]
        [Header("Show Anim")]
        [SerializeField, ShowIf(nameof(ShowButtonAnim))]
        private ShowAnimType showAnimType = ShowAnimType.None;
        
        [SerializeField, HideIf(nameof(showAnimType), ShowAnimType.None)]
        private float showAnimTime = .35f;
        
        [SerializeField, ShowIf(nameof(showAnimType), ShowAnimType.FromScale)]
        private float initScale = 0;

        [SerializeField, ShowIf(nameof(showAnimType), ShowAnimType.FromScale)]
        private bool separateAxisShowAnim;
        
        [ShowIf(nameof(ShowShowAnimEasingType))]
        public EasingType showAnimEasingType = EasingType.OutQuad;
        
        [ShowIf(nameof(UseShowAnimCurve))]
        public AnimationCurve showAnimCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [ShowIf(nameof(ShowSeparateShowAnimEasingType))]
        public EasingType showAnimXAxisEasingType = EasingType.OutQuad;
        
        [ShowIf(nameof(UseXAxisShowAnimCurve))]
        public AnimationCurve showAnimXAxisCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [ShowIf(nameof(ShowSeparateShowAnimEasingType))]
        public EasingType showAnimYAxisEasingType = EasingType.OutQuad;
        
        [ShowIf(nameof(UseYAxisShowAnimCurve))]
        public AnimationCurve showAnimYAxisCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        #endregion
        
        #region Press Anim

        [Space]
        [Header("Press Anim")]
        [SerializeField, ShowIf(nameof(ShowButtonAnim))]
        private PressAnimType pressAnimType = PressAnimType.None;

        [SerializeField, HideIf(nameof(pressAnimType), PressAnimType.None)]
        private float pressAnimTime = .2f;
        
        [SerializeField, HideIf(nameof(pressAnimType), PressAnimType.None)]
        private float releaseAnimTime = .2f;
        
        [SerializeField, ShowIf(nameof(pressAnimType), PressAnimType.Scale)]
        private Vector2 pressedTargetScale = Vector2.one * .95f;

        [SerializeField, HideIf(nameof(pressAnimType), PressAnimType.None)]
        private EasingType pressAnimEasingType = EasingType.OutQuad;
        
        [SerializeField, ShowIf(nameof(UsePressAnimCurve))]
        private AnimationCurve pressAnimCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [SerializeField, HideIf(nameof(pressAnimType), PressAnimType.None)]
        private EasingType releaseAnimEasingType = EasingType.OutQuad;
        
        [SerializeField, ShowIf(nameof(UseReleaseAnimCurve))]
        private AnimationCurve releaseAnimCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        #endregion
        
        #region Hide Anim
        
        [Space]
        [Header("Hide Anim")]
        [SerializeField, ShowIf(nameof(ShowButtonAnim))]
        private HideAnimType hideAnimType = HideAnimType.None;
        
        [SerializeField, HideIf(nameof(hideAnimType), HideAnimType.None)]
        private float hideAnimTime = .35f;
        
        [ShowIf(nameof(hideAnimType), HideAnimType.ToScale)]
        public float targetScale = 0;
        
        [ShowIf(nameof(hideAnimType), HideAnimType.ToScale)]
        public bool separateAxisHideAnim;
        
        [ShowIf(nameof(ShowHideAnimEasingType))]
        public EasingType hideAnimEasingType = EasingType.OutQuad;
        
        [ShowIf(nameof(UseHideAnimCurve))]
        public AnimationCurve hideAnimCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [ShowIf(nameof(ShowSeparateHideAnimEasingType))]
        public EasingType hideAnimXAxisEasingType = EasingType.OutQuad;
        
        [ShowIf(nameof(UseXAxisHideAnimCurve))]
        public AnimationCurve hideAnimXAxisCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [ShowIf(nameof(ShowSeparateHideAnimEasingType))]
        public EasingType hideAnimYAxisEasingType = EasingType.OutQuad;
        
        [ShowIf(nameof(UseYAxisHideAnimCurve))]
        public AnimationCurve hideAnimYAxisCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        #endregion

        #endregion
        
        [SerializeField] private ButtonExtension btn;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Sound")] 
        [SerializeField] private TypeSound typeSound = TypeSound.Button;
        
        private void Reset()
        {
            btn = GetComponent<ButtonExtension>();
            canvasGroup = GetComponent<CanvasGroup>();
            btn.transition = Selectable.Transition.None;
            btn.buttonAnim = this;
        }

        public void ShowAnim(Action onComplete = null)
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 1f;
            transform.DOKill();
            transform.localScale = Vector3.one;
            
            if (showAnimType == ShowAnimType.None)
            {
                onComplete?.Invoke();
                return;
            }

            Ease dotweenEase = (Ease) showAnimEasingType;
            
            switch (showAnimType)
            {
                case ShowAnimType.FadeIn:
                    if (canvasGroup)
                    {
                        canvasGroup.alpha = 0;

                        var tween = canvasGroup.DOFade(1, showAnimTime)
                            .SetUpdate(unscaleTime)
                            .OnComplete(() => onComplete?.Invoke());
                        
                        if (UseShowAnimCurve)
                            tween.SetEase(showAnimCurve);
                        else
                            tween.SetEase(dotweenEase);
                    }
                    
                    break;
                case ShowAnimType.FromScale:
                    if (canvasGroup)
                    {
                        canvasGroup.alpha = 0;
                        canvasGroup.DOFade(1, showAnimTime * .75f).SetUpdate(unscaleTime);
                    }
                    
                    transform.localScale = Vector3.one * initScale;

                    if (separateAxisShowAnim)
                    {
                        Ease dotweenXEase = (Ease) showAnimXAxisEasingType;
                        Ease dotweenYEase = (Ease) showAnimYAxisEasingType;
                        
                        var xTween = transform.DOScaleX(1, showAnimTime)
                            .SetUpdate(unscaleTime)
                            .OnComplete(() => onComplete?.Invoke());
                        
                        if (UseXAxisShowAnimCurve)
                            xTween.SetEase(showAnimXAxisCurve);
                        else
                            xTween.SetEase(dotweenXEase);

                        var yTween = transform.DOScaleY(1, showAnimTime)
                            .SetUpdate(unscaleTime);

                        if (UseYAxisShowAnimCurve)
                            yTween.SetEase(showAnimYAxisCurve);
                        else
                            yTween.SetEase(dotweenYEase);
                    }
                    else
                    {
                        var tween = transform.DOScale(1, showAnimTime)
                            .SetUpdate(unscaleTime)
                            .OnComplete(() => onComplete?.Invoke());
                        
                        if (UseShowAnimCurve)
                            tween.SetEase(showAnimCurve);
                        else
                            tween.SetEase(dotweenEase);
                    }
                    
                    break;
            }
        }
        
        public void HideAnim(Action onComplete = null)
        {
            canvasGroup.DOKill();
            transform.DOKill();
            
            if (hideAnimType == HideAnimType.None)
            {
                onComplete?.Invoke();
                return;
            }
            
            Ease dotweenEase = (Ease) hideAnimEasingType;
            switch (hideAnimType)
            {
                case HideAnimType.FadeOut:
                    if (canvasGroup)
                    {
                        canvasGroup.alpha = 1;

                        var tween = canvasGroup.DOFade(0, hideAnimTime)
                            .SetUpdate(unscaleTime)
                            .OnComplete(() => onComplete?.Invoke());
                        
                        if (UseHideAnimCurve)
                            tween.SetEase(hideAnimCurve);
                        else
                            tween.SetEase(dotweenEase);
                    }
                    
                    break;
                case HideAnimType.ToScale:
                    if (canvasGroup)
                    {
                        canvasGroup.alpha = 1;
                        canvasGroup.DOFade(0, hideAnimTime * .75f).SetUpdate(unscaleTime);
                    }

                    if (separateAxisHideAnim)
                    {
                        Ease dotweenXEase = (Ease) hideAnimXAxisEasingType;
                        Ease dotweenYEase = (Ease) hideAnimYAxisEasingType;
                        
                        var xTween = transform.DOScaleX(targetScale, hideAnimTime)
                            .SetUpdate(unscaleTime)
                            .OnComplete(() => onComplete?.Invoke());
                        
                        if (UseXAxisHideAnimCurve)
                            xTween.SetEase(hideAnimXAxisCurve);
                        else
                            xTween.SetEase(dotweenXEase);

                        var yTween = transform.DOScaleY(targetScale, hideAnimTime)
                            .SetUpdate(unscaleTime);

                        if (UseYAxisHideAnimCurve)
                            yTween.SetEase(hideAnimYAxisCurve);
                        else
                            yTween.SetEase(dotweenYEase);
                    }
                    else
                    {
                        var tween = transform.DOScale(targetScale, hideAnimTime)
                            .SetUpdate(unscaleTime)
                            .OnComplete(() => onComplete?.Invoke());
                        
                        if (UseHideAnimCurve)
                            tween.SetEase(hideAnimCurve);
                        else
                            tween.SetEase(dotweenEase);
                    }
                    
                    break;
            }
        }

        void PointerDownAnim()
        {
            if (pressAnimType == PressAnimType.None)
                return;

            transform.DOKill();
            Vector3 pressedTargetScale = this.pressedTargetScale;
            pressedTargetScale.z = 1;
            Ease dotweenEase = (Ease) pressAnimEasingType;

            switch (pressAnimType)
            {
                case PressAnimType.Scale:
                    var tween = transform.DOScale(pressedTargetScale, pressAnimTime)
                        .SetUpdate(unscaleTime);

                    if (UsePressAnimCurve)
                        tween.SetEase(pressAnimCurve);
                    else
                        tween.SetEase(dotweenEase);

                    break;
            }
        }

        void PointerUpAnim()
        {
            if (pressAnimType == PressAnimType.None)
                return;
            
            transform.DOKill();
            Ease dotweenEase = (Ease) releaseAnimEasingType;
            
            switch (pressAnimType)
            {
                case PressAnimType.Scale:
                    var tween = transform.DOScale(Vector3.one, releaseAnimTime)
                        .SetUpdate(unscaleTime);

                    if (UseReleaseAnimCurve)
                        tween.SetEase(releaseAnimCurve);
                    else
                        tween.SetEase(dotweenEase);

                    break;
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!btn.interactable) 
                return;
            
            PointerDownAnim();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!btn.interactable) 
                return;
            
            PointerUpAnim();
            AudioAssistant.Shot(typeSound);
        }
    }
}