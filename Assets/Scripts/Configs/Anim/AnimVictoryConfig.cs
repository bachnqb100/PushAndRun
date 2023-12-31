using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(menuName = "Configs/Anim Item/Victory", fileName = "Anim Victory Config")]
    public class AnimVictoryConfig : ScriptableObject
    {
        public VictoryAnimType type;
        public string animName;
        [TextArea]
        public string description;
        
        [PreviewField]
        public Sprite icon;

        public bool isExclusive;
        public bool isAdsUnlock;

        public int price;
        
    }
}