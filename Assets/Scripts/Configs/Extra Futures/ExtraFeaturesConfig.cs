using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Configs.Extra_Futures
{
    [CreateAssetMenu(menuName = "Configs/Extra Features", fileName = "Extra Features Config")]
    public class ExtraFeaturesConfig : ScriptableObject
    {
        [Title("Online Gift")] 
        public int onlineRewardInterval = 600;
        public int onlineRewardValue = 1000;

        [PropertySpace] 
        [Title("Lucky Wheel")] 
        public int freeSpinInterval = 14400;
        public List<int> luckyWheelCoinRewardValues;
    
        [PropertySpace] 
        [Title("Daily Reward")] 
        public List<int> dailyRewardCoinRewardValues;
    }
}