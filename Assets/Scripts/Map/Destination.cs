using System;
using Sigtrap.Relays;
using UnityEngine;

namespace DefaultNamespace.Map
{
    public class Destination : MonoBehaviour
    {
        public Relay OnTriggerDestinationByPlayer = new Relay();
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("WINNNNN");
                OnTriggerDestinationByPlayer.Dispatch();
                //TODO: Logic win + effect 
            }
        }
    }
}