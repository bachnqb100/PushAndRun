using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Map
{
    public class MapUtils : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(1f, 1f, 1f);
#if UNITY_EDITOR
        [Button]
        void PlaceItOnCollision()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1000f))
            {
                transform.position = hitInfo.point + offset;
            }
        }
        
        [Button]
        void PlaceItOnCollisionPrefab()
        {
            if (gameObject.scene.GetPhysicsScene().Raycast(transform.position, Vector3.down, out var hitInfo, 1000f))
            {
                transform.position = hitInfo.point + offset;
            }
        }
#endif
    }
}