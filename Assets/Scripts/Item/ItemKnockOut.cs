using System;
using DefaultNamespace;
using UnityEngine;

namespace Item
{
    public class ItemKnockOut : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventGlobalManager.Instance.OnEnemyKnockout.Dispatch();
                
                Destroy(gameObject);
            }
        }
    }
}