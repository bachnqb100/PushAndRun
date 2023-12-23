using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MoneyTween : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
    
        private int _currentMoney;

        private void OnEnable()
        {
            
            EventGlobalManager.Instance.OnMoneyChange.AddListener(UpdateMoney);
        }

        private void Start()
        {
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
    }
}