using System;
using DefaultNamespace;
using UnityEngine;

namespace Item
{
    public class ItemConsumeFitness : MonoBehaviour
    {
        [SerializeField] private float fitnessConsumeValue = 1f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventGlobalManager.Instance.OnPlayerCollectConsumeFitness.Dispatch(fitnessConsumeValue);
                
                Destroy(gameObject);
            }
        }
    }
}