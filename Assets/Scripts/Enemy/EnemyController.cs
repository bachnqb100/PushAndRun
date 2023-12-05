using System.Collections;
using RootMotion.Demos;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Enemy
{
    public class EnemyController : UserControlThirdPerson
    {
        public Transform moveTarget;
        public Navigator navigator;
        
        [Space]
        [Title("Puppet")]
        [SerializeField] private CharacterPuppet characterPuppet;

        [SerializeField] private CharacterAnimationThirdPerson characterAnimation;
        
        [Title("Move")]
        [SerializeField, Range(0.1f, 3f)] private float moveSpeed = 1f;
        
        [Title("Jump")]
        [SerializeField] private float offsetPositionCheckJump = 2f;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float checkGroundDistance = 10f;
        
        private bool _jump;
        private bool _chase;
        

        private Vector3 GetCheckJumpPos()
        {
            Vector3 checkJumpPos = transform.position + transform.forward * offsetPositionCheckJump;
            checkJumpPos.y += 2f;
            return checkJumpPos;
        }

        protected override void Start()
        {
            base.Start();

            navigator.Initiate(transform);
            characterAnimation.SetAnimSpeed(moveSpeed);
        }

        protected override void Update ()
        {
	        
            float moveSpeed = walkByDefault? 0.5f: 1f;

            // If using Unity Navigation
            if (navigator.activeTargetSeeking && _chase && characterPuppet.onGround)
            {
                navigator.Update(moveTarget.position);
                state.move = navigator.normalizedDeltaPosition * moveSpeed;
            }

            Jump();
            
            state.jump = canJump && _jump;
        }

        [Button]
        public void SetSpeed(float speed)
        {
            characterAnimation.SetAnimSpeed(speed);
        }

        void Jump()
        {
            if (!characterPuppet.onGround) return;
            if (!_chase) return;
            
            RaycastHit[] raycastHits = new RaycastHit[5];
            //Jump
            if (Physics.RaycastNonAlloc(GetCheckJumpPos(), Vector3.down, raycastHits, checkGroundDistance,
                    groundLayerMask) == 0)
            {
                StartJump();
            }
        }

        // Visualize the navigator
        void OnDrawGizmos()
        {
            if (navigator.activeTargetSeeking) navigator.Visualize();
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(GetCheckJumpPos(), GetCheckJumpPos() + Vector3.down * checkGroundDistance);
        }
        

        [Button]
        void SetStatusChase(bool enable)
        {
            _chase = enable;
        }
        
        [Button]
        void StartJump()
        {
            StartCoroutine(SetJump());
        }

        IEnumerator SetJump()
        {
            Debug.Log("Jump");
            _jump = true;
            //navigator.SetState(Navigator.State.Idle);
            navigator.StopMove();
            yield return new WaitForEndOfFrame();
            _jump = false;
        }
    }
}