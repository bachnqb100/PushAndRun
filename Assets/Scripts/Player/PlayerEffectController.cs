﻿using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    public class PlayerEffectController : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> runTrails;
        [SerializeField] private GameObject effectSprint;
        [SerializeField] private GameObject effectJog;

        [SerializeField] private GameObject shield;

        
        public void EnableRunTrail()
        {
            if (!GameController.Instance.IsPlaying) return; 
            
            foreach (var item in runTrails)
            {
                if (!item.isPlaying)
                {
                    item.Clear();
                    item.Play();
                }
            }
        }

        public void DisableRunTrail()
        {
            foreach (var item in runTrails)
            {
                if (item.isPlaying)
                    item.Stop();
            }
        }

        public void StopEffectSprintJog()
        {
            SetStatusEffectSprint(false);
            SetStatusEffectJog(false);
        }

        public void SetStatusEffectSprint(bool enable)
        {
            if (effectSprint.activeSelf != enable)
                effectSprint.SetActive(enable);
        }
        
        public void SetStatusEffectJog(bool enable)
        {
            if (effectJog.activeSelf != enable)
                effectJog.SetActive(enable);
        }

        public void EnableShield()
        {
            shield.SetActive(true);
        }
        public void DisableShield()
        {
            shield.SetActive(false);
        }
        
        
    }
}