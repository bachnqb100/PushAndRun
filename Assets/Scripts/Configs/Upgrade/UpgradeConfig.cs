using UnityEngine;

namespace DefaultNamespace.Configs.Upgrade
{
    [CreateAssetMenu(menuName = "Configs/Upgrade", fileName = "Upgrade Config")]
    public class UpgradeConfig : ScriptableObject
    {
        public float upgradeSpeedValue = 0.05f;
        public float upgradeFitnessValue = 0.05f;
        public float upgradeIncreaseFitnessValue = 0.05f;
        public float upgradeDecreaseFitnessValue = 0.05f;
        public int upgradeMoneyIncomeValue = 1;
    }
}