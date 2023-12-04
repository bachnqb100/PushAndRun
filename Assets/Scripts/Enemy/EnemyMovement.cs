using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [Title("Components")]
        [SerializeField] private NavMeshAgentExtension navmeshAgent;
        [SerializeField] private AnimationController animationController;
        
        [SerializeField]
        private SerializedDictionary<string, float> jumpAnimDurationMap = new SerializedDictionary<string, float>();

        [Title("Stats")] [SerializeField] private float speed = 1f;

        private bool _isJumping;

        private void Awake()
        {
            Init();
        }

        IEnumerator Start()
        {
            navmeshAgent.agent.autoTraverseOffMeshLink = false;
            _isJumping = false;

            while (true)
            {
                if (navmeshAgent.agent.isOnOffMeshLink && !_isJumping)
                    DOVirtual.DelayedCall(Jump(), () =>
                    {
                        navmeshAgent.agent.CompleteOffMeshLink();
                        _isJumping = false;
                        animationController.SetStatusRootMotion(false);
                    }).SetTarget(this);
                
                animationController.SetMoveSpeedAnim(navmeshAgent.Speed / speed);
                
                yield return null;
            }
        }

        void Init()
        {
            navmeshAgent.agent.speed = speed;
            
        }

        float Jump()
        {
            animationController.SetStatusRootMotion(true);
            _isJumping = true;
            var jumpData = jumpAnimDurationMap.ElementAt(UnityEngine.Random.Range(0, jumpAnimDurationMap.Count));
            
            animationController.Jump(jumpData.Key);
            
            return jumpData.Value;
        }


        private void Reset()
        {
            navmeshAgent = GetComponent<NavMeshAgentExtension>();
        }
    }
}
