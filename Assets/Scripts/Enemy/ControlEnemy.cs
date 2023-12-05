using System.Collections;
using RootMotion.Demos;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class ControlEnemy : UserControlThirdPerson
    {
        [Title("Components")] 
        [SerializeField] private NavMeshAgentExtension navMeshAgentExtension;
        
        [Title("Stats")]
        [SerializeField] private bool runByDefault = true;

        [Header("Test")] [SerializeField] private Transform target;
        
        private bool _jump;
        private Animator _animator;

        protected override void Start()
        {
            base.Start();
            _jump = false;

            if (_animator == null)
            {
                //_animator = 
            }
        }

        protected override void Update()
        {
            //base.Update();
            state.jump = canJump && _jump;
            
            
        }

        [Button]
        void Chase(bool enable)
        {
            if (enable)
            {
                navMeshAgentExtension.GoTo(target);
                //navMeshAgentExtension.agent.path.
            }
            else
            {
                navMeshAgentExtension.SetAgentEnable(false);
            }
        }

        [Button]
        void StartJump()
        {
            StartCoroutine(SetJump());
        }

        IEnumerator SetJump()
        {
            _jump = true;
            yield return null;
            _jump = false;
        }
    }
}