using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MoneyTween : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;

        [Header("Effects")] 
        [SerializeField] private Transform startTransform;
        [SerializeField] private RectTransform moneyChangeEffect;
        [SerializeField] private CanvasGroup moneyChangeCanvasGroup;
        [SerializeField] private float moneyChangeDuration = 1f;
        [SerializeField] private TMP_Text moneyChangeText;
    
        private int _currentMoney;

        private void OnEnable()
        {
            EventGlobalManager.Instance.OnMoneyChange.AddListener(UpdateMoney);
            _currentMoney = GameManager.Instance.GameData.userData.money;
            moneyText.text = _currentMoney.ToFormatString();
        }

        private void Start()
        {
            EventGlobalManager.Instance.OnMoneyChange.AddListener(UpdateMoney);
            _currentMoney = GameManager.Instance.GameData.userData.money;
            moneyText.text = _currentMoney.ToFormatString();
        }

        private void OnDisable()
        {
            if (EventGlobalManager.Instance)
                EventGlobalManager.Instance.OnMoneyChange.RemoveListener(UpdateMoney);
        }

        void UpdateMoney(bool success)
        {
            DOTween.Kill(this);
            if (success)
            {
                int tmp = _currentMoney;
                DOTween.To(() => tmp, UpdateMoneyText, GameManager.Instance.GameData.userData.money, .2f)
                    .SetEase(Ease.Linear).SetTarget(this);
                
                if (tmp > GameManager.Instance.GameData.userData.money) StartAnimMoneyChange(tmp -  GameManager.Instance.GameData.userData.money);
            }
            else
            {
                // Error anim
            }
        }

        void UpdateMoneyText(int money)
        {
            _currentMoney = money;
            moneyText.text = money.ToFormatString();
        }

        [Button]
        void StartAnimMoneyChange(int value)
        {
            moneyChangeEffect.DOKill(true);
            moneyChangeEffect.gameObject.SetActive(true);
            
            moneyChangeText.text = " - " + value;

            moneyChangeEffect.localPosition = startTransform.localPosition;

            moneyChangeEffect.transform.DOLocalMoveY(30f, moneyChangeDuration);
            DOVirtual.Float(1f, 0f, moneyChangeDuration, x => moneyChangeCanvasGroup.alpha = x)
                .OnComplete(() => moneyChangeEffect.gameObject.SetActive(false)).SetTarget(moneyChangeEffect);
        }
                
    }
}