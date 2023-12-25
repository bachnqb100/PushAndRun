using UnityEngine;

namespace DefaultNamespace.Map
{
    public class SpawnItemPos : MonoBehaviour
    {
        private GameObject _item;
        
        public GameObject Item { get => _item; set => _item = value; }
    }
}