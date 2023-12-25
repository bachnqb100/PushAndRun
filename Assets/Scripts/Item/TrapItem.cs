using System;
using DefaultNamespace.Enemy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Item
{
    public class TrapItem : MonoBehaviour
    {
        [SerializeField] private float radius = 3f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float maxHeight = 3f;
        [SerializeField] private float airSpeed = 1f;
        
        [Button]
        public void Init()
        {
            Vector3 initPos = transform.position;
            Vector3 endPos = GetPosOnGround();
            Vector3 mid = (initPos + endPos) / 3;
            mid.y += maxHeight;
            DOVirtual.Float(0f, 1f, 1 / airSpeed, t =>
            {
                transform.position = CalculateProjectilePosition(initPos, mid, endPos, t);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent<IKnockOut>(out var e);

            if (e != null)
            {
                e.KnockOut();
                
                Destroy(gameObject);
            }
        }

        Vector3 GetPosOnGround()
        {
            Vector3 newRandomPos = transform.position;
            newRandomPos.y += 5f;

            Vector2 randomDir = Random.insideUnitCircle;
            
            newRandomPos.x += randomDir.x * radius;
            newRandomPos.z += randomDir.y * radius;

            if (Physics.Raycast(newRandomPos, Vector3.down, out var hitInfo, 10f, groundLayer))
            {
                return hitInfo.point;
            }

            return transform.position;
        }
        
        
        Vector3 CalculateProjectilePosition(Vector3 start, Vector3 mid, Vector3 end, float t)
        {
            // Phương trình chuyển động ném xiên
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 result = uuu * start; // (1-t)^3 * start
            result += 3 * uu * t * mid; // 3(1-t)^2 * t * mid
            result += 3 * u * tt * end; // 3(1-t) * t^2 * end
            result += ttt * end; // t^3 * end

            return result;
        }
    }
}