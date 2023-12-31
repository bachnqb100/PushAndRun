using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(menuName = "Configs/Anim Item/Group/Anim Main Group Config", fileName = "Anim Main Group Config")]
    public class AnimMainGroupConfig : ScriptableObject
    {
        [TableList(DrawScrollView = false, ShowPaging = true, NumberOfItemsPerPage = 25, ShowIndexLabels = false)]
        public List<AnimMainConfig> configs;

        public AnimMainConfig GetAnimMainConfig(MainAnimType type)
            => configs.First(x => x.type == type);
    }
}