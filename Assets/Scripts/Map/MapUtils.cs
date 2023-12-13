using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Map
{
    public class MapUtils : MonoBehaviour
    {
#if UNITY_EDITOR
        [Button]
        void PlaceItOnCollision()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1000f))
            {
                transform.position = hitInfo.point;
            }
        }
#endif
    }
}