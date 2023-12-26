using RootMotion;
using UnityEngine;

namespace DefaultNamespace.Configs.Upgrade
{
    [CreateAssetMenu(menuName = "Configs/Upgrade/Upgrade Item", fileName = "Upgrade Item")]
    public class UpgradeItemConfig : ScriptableObject
    {
        public UpgradeType type;
        public string nameUpgrade;
        [TextArea] public string description;
        public int moneyUpgrade = 100;
        
        public UpgradeValueType valueType;
        
        [ShowIf(nameof(valueType), UpgradeValueType.Float)]
        public float upgradeFloatValue = 0.05f;
        [ShowIf(nameof(valueType), UpgradeValueType.Int)]
        public int upgradeIntValue = 1;
    }
}