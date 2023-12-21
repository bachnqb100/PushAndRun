using System;
using DG.Tweening;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Map
{
    public class Destination : MonoBehaviour
    {
        [SerializeField] private Image count;
        [SerializeField] private float countDownDuration = 3f;
        [SerializeField] private float lostTargetDuration = 1f;
        
        public Relay OnTriggerDestinationByPlayer = new Relay();

        private void OnEnable()
        {
            count.fillAmount = 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter" + other.name);
            if (!other.gameObject.CompareTag("Player")) return;
            this.DOKill();
            DOVirtual.Float(0f, 1f, countDownDuration, x =>
            {
                count.fillAmount = x;
            }).OnComplete(() =>
            {
                OnTriggerDestinationByPlayer.Dispatch();
                    
                Debug.Log("WINNNNN");
                //TODO: Logic win + effect 
            }).SetTarget(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            this.DOKill();

            DOVirtual.Float(count.fillAmount, 0f, lostTargetDuration, x =>
            {
                count.fillAmount = x;
            }).SetTarget(this);
        }
    }
}