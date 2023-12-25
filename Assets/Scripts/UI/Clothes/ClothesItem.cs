using System;
using DefaultNamespace.Skin;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.Clothes
{
    public class ClothesItem : MonoBehaviour
    {
        [SerializeField] private ClothesType type;
        [SerializeField] private TMP_Text typeText;
        
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private ButtonExtension btnPre;
        [SerializeField] private ButtonExtension btnNext;

        private void Start()
        {
            typeText.text = type.ToString();
            UpdateText();
        }

        private void OnEnable()
        {
            btnPre.onClick.AddListener(PreviousColorItem);
            btnNext.onClick.AddListener(NextColorItem);
        }

        private void OnDisable()
        {
            btnPre.onClick.RemoveListener(PreviousColorItem);
            btnNext.onClick.RemoveListener(NextColorItem);
        }

        void UpdateText()
        {
            valueText.text = GameManager.Instance.GameData.userData.clothesColorMap[type].ToString();
        }

        void NextColorItem()
        {
            int nextColor = (int)GameManager.Instance.GameData.userData.clothesColorMap[type] + 1;
            nextColor = nextColor >= Enum.GetValues(typeof(ClothesColorType)).Length ? 0 : nextColor;
            //nextColor = Mathf.Clamp(nextColor, 0, Enum.GetValues(typeof(ClothesColorType)).Length - 1);
            SkinManager.Instance.UpdateColorClothes(type,(ClothesColorType) nextColor);
            UpdateText();
        }
        
        void PreviousColorItem()
        {
            int nextColor = (int)GameManager.Instance.GameData.userData.clothesColorMap[type] - 1;
            nextColor = nextColor < 0 ? Enum.GetValues(typeof(ClothesColorType)).Length - 1 : nextColor;
            //nextColor = Mathf.Clamp(nextColor, 0, Enum.GetValues(typeof(ClothesColorType)).Length - 1);
            SkinManager.Instance.UpdateColorClothes(type,(ClothesColorType) nextColor);
            UpdateText();
        }
    }
}