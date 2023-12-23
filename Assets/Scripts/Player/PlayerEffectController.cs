using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    public class PlayerEffectController : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> runTrails;

        
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
    }
}