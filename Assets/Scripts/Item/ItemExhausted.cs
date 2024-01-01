using System;
using DefaultNamespace;
using DefaultNamespace.Audio;
using UnityEngine;

namespace Item
{
    public class ItemExhausted : MonoBehaviour
    {
        [SerializeField] private float exhaustedDuration = 1f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventGlobalManager.Instance.OnPlayerExhausted.Dispatch(exhaustedDuration);
                
                AudioAssistant.Shot(TypeSound.ExhaustedItem);
                Destroy(gameObject);
            }
        }
    }
}