using System;
using DefaultNamespace;
using UnityEngine;

namespace Item
{
    public class ItemShield : MonoBehaviour
    {
        [SerializeField] private float shieldDuration = 5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventGlobalManager.Instance.OnPlayerCollectShield.Dispatch(shieldDuration);
                
                Destroy(gameObject);
            }
        }
    }
}