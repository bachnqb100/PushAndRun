using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(menuName = "Configs/Anim Item/Group/Anim Victory Group Config", fileName = "Anim Victory Group Config")]
    public class AnimVictoryGroupConfig : ScriptableObject
    {
        [TableList(DrawScrollView = false, ShowPaging = true, NumberOfItemsPerPage = 25, ShowIndexLabels = false)]
        public List<AnimVictoryConfig> configs;
        
        public AnimVictoryConfig GetAnimVictoryConfig(VictoryAnimType type) =>
            configs.First(x => x.type == type);
    }
}