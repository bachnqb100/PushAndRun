using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Configs.Skin
{
    [CreateAssetMenu(menuName = "Configs/Skin", fileName = "Skin Color Config")]
    public class SkinConfig : ScriptableObject
    {
        [SerializeField]
        [TableList(DrawScrollView = false, ShowPaging = true, NumberOfItemsPerPage = 25, ShowIndexLabels = true)]
        public List<ClothesData> clothesData = new List<ClothesData>();

        public ClothesColorType? GetClothesColorType(ClothesType type)
        {
            foreach (var item in clothesData)
            {
                if (item.clothesType == type) return item.clothesColorType;
            }
            return null;
        }
    }
}