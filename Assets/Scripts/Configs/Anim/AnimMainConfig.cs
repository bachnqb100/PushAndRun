using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(menuName = "Configs/Anim Item/Main", fileName = "Anim Main Config")]
    public class AnimMainConfig : ScriptableObject
    {
        public MainAnimType type;
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