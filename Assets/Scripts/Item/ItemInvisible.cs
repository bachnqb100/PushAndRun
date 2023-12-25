using System;
using DefaultNamespace;
using UnityEngine;

namespace Item
{
    public class ItemInvisible : MonoBehaviour
    {
        [SerializeField] private float invisibleDuration = 5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventGlobalManager.Instance.OnPlayerCollectShield.Dispatch(invisibleDuration);
                
                Destroy(gameObject);
            }
        }
    }
}