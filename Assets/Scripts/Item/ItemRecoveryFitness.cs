using System;
using DefaultNamespace;
using DefaultNamespace.Audio;
using UnityEngine;

namespace Item
{
    public class ItemRecoveryFitness : MonoBehaviour
    {
        [SerializeField] private float recoveryValue = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventGlobalManager.Instance.OnPlayerCollectRecoveryFitness.Dispatch(recoveryValue);
                
                AudioAssistant.Shot(TypeSound.RecoverItem);
                Destroy(gameObject);
            }
        }
    }
}