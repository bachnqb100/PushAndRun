using System;
using System.Collections.Generic;
using DefaultNamespace.Audio;
using DG.Tweening;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Map
{
    public class Destination : MonoBehaviour
    {
        [Header("Local Canvas")]
        [SerializeField] private GameObject localCanvas;
        [SerializeField] private Image count;
        [SerializeField] private float countDownDuration = 3f;
        [SerializeField] private float lostTargetDuration = 1f;

        [Header("Effects")] 
        [SerializeField] private List<ParticleSystem> effects;
        
        [Header("Indicator")]
        [SerializeField] private Waypoint_Indicator indicator;
        
        public Relay OnTriggerDestinationByPlayer = new Relay();

        private bool _isShowingEffect;

        private float _cdTimeShotAudio;

        private float _shotConfettiRate;

        private void OnEnable()
        {
            _isShowingEffect = false;
            count.fillAmount = 0f;

            _shotConfettiRate = GameController.Instance.ShotConfettiRate;
            _cdTimeShotAudio = _shotConfettiRate;
            
            localCanvas.SetActive(true);
            indicator.enableStandardTracking = true;
        }

        private void Update()
        {
            if (!_isShowingEffect) return;

            _cdTimeShotAudio -= Time.deltaTime;

            if (_cdTimeShotAudio <= 0f)
            {
                ShotAudioConfetti();

                _cdTimeShotAudio = _shotConfettiRate;
            }

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
                
                ShowEffect();
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

        void ShowEffect()
        {
            _isShowingEffect = true;
            localCanvas.SetActive(false);
            indicator.enableStandardTracking = false;
            foreach (var item in effects)
            {
                item.Play();
            }
        }

        void ShotAudioConfetti()
        {
            AudioAssistant.Shot(TypeSound.Confetti1);

            DOVirtual.DelayedCall(Random.Range(0.1f, 0.5f), () => AudioAssistant.Shot(TypeSound.Confetti2));
        }
    }
}