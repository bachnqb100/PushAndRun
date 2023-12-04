using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestMechanic : MonoBehaviour
    {
        [SerializeField] private float collisionRange = 3f;
        [SerializeField] private LayerMask enemyLayerMask;


        private void Update()
        {
            var collisions = new Collider[10];

            if(Physics.OverlapSphereNonAlloc(transform.position, collisionRange, collisions, enemyLayerMask)  > 0)
            {
                Debug.Log("Detecting collision");
                Debug.Log(collisions[0].name);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, collisionRange);
        }
    }
}