using System;
using System.Collections;
using FieldOfViewAsset;
using RootMotion.Demos;
using RootMotion.Dynamics;
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

        [Title("Field Of View")] 
        [SerializeField] private FieldOfView fov;
        [SerializeField] private float detectDuration = 5f;
        [SerializeField] private float lostDuration = 5f;
        
        [Title("Behaviors")]
        [SerializeField] private bool canChaseBehavior = true;
        [SerializeField] private bool canJumpBehavior = true;
        
        
        [Title("Test")]
        [SerializeField] private BehaviourPuppet behaviourPuppet;
        
        private bool _jump;
        private bool _chase;
        
        //fov
        private GameObject _currentTarget;

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
            SetSpeed(moveSpeed);

            _currentTarget = null;
            
            //FOV
            //InitFOV();
            StopFOV();
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
            
            LookAtTarget();

            Jump();
            
            state.jump = canJump && _jump;
        }

        private void OnEnable()
        {
            InitFOV();
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnregisterEvent();
        }

        void InitFOV()
        {
            fov.TargetSpotted += OnSpotedTarget;
            fov.TargetDetected += OnDetectedTarget;
            fov.TargetLost += OnLostTarget;

            fov.DetectionTime = detectDuration;
            fov.CoolDownTime = lostDuration;
        }
        void SetSpeed(float speed)
        {
            characterAnimation.SetAnimSpeed(speed);
        }

        void Jump()
        {
            if (!canJumpBehavior) return;
            if (!characterPuppet.onGround) return;
            if (!_chase) return;
            
            RaycastHit[] raycastHits = new RaycastHit[5];
            //Jump
            if (Physics.RaycastNonAlloc(GetCheckJumpPos(), Vector3.down, raycastHits, checkGroundDistance,
                    groundLayerMask) == 0)
            {
                StartJump();
            }
            
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
            _chase = canChaseBehavior && enable;
        }
        
        void LookAtTarget()
        {
            if (_currentTarget)
            {
                var dir = _currentTarget.transform.position - transform.position;
                state.move = dir.normalized * 0.00001f;
                state.lookPos = _currentTarget.transform.position;
            }
        }

        #region FOV Handler
        
        void OnSpotedTarget(GameObject target)
        {
            Debug.Log("Spoted Target");
            _currentTarget = target;
            
            characterAnimation.SetLookAt();
        }

        void OnDetectedTarget(GameObject target)
        {
            Debug.Log("Detected Target");
            _currentTarget = target;
        }

        void OnLostTarget(GameObject target)
        {
            Debug.Log("Lost Target");
            _currentTarget = null;
            characterAnimation.SetLookAround();
        }

        #endregion

        void RegisterEvent()
        {
            behaviourPuppet.onRegainBalance.unityEvent.AddListener(StartFOV);
            behaviourPuppet.onRegainBalance.unityEvent.AddListener(StartBehaviour);
            
            behaviourPuppet.onLoseBalance.unityEvent.AddListener(StopFOV);
        }

        void UnregisterEvent()
        {
            behaviourPuppet.onRegainBalance.unityEvent.RemoveListener(StartFOV);
            behaviourPuppet.onRegainBalance.unityEvent.RemoveListener(StartBehaviour);
            
            behaviourPuppet.onLoseBalance.unityEvent.RemoveListener(StopFOV);
        }
        
        [Button]
        void TestPuppet()
        {
            behaviourPuppet.SetState(BehaviourPuppet.State.Unpinned);
            behaviourPuppet.KillStart();
        }
        
        //Behavior
        void StartFOV()
        {
            Debug.Log("StartFOV");
            fov.enabled = true;
        }

        void StopFOV()
        {
            Debug.Log("StopFOV");
            fov.enabled = false;
        }

        void StartBehaviour()
        {
            SetStatusChase(true);
        }
        
    }
}