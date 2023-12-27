using System;
using System.Collections;
using FieldOfViewAsset;
using Player;
using RootMotion.Demos;
using RootMotion.Dynamics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Enemy
{
    public class EnemyController : UserControlThirdPerson, IFallable, IKnockOut
    {
        public Navigator navigator;
        
        [Space]
        [Title("Puppet")]
        [SerializeField] private CharacterPuppet characterPuppet;

        [SerializeField] private AnimController characterAnimation;
        
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
        [SerializeField] private ChaseType chaseType;
        [SerializeField] private bool canJumpBehavior = true;
        [SerializeField] private float distanceToChangeTarget = 1f;
        
        
        [Title("Test")]
        [SerializeField] private BehaviourPuppet behaviourPuppet;
        
        private bool _jump;
        private bool _chase;
        
        //fov
        private GameObject _currentTarget;
        
        //chase
        private Transform _currentTargetMovement;
        
        //gameplay
        private bool _isFell;
        
        public bool IsFell => _isFell;

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
            _isFell = false;
            //FOV
            //InitFOV();
            StopFOV();
        }

        protected override void Update ()
        {

            Move();
            
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
            characterAnimation.AnimSpeedMultiplier = speed;
        }

        void Move()
        {
            float moveSpeed = walkByDefault ? 0.5f : 1f;
            
            if (navigator.activeTargetSeeking && _chase && characterPuppet.onGround)
            {
                if (chaseType == ChaseType.Hunt)
                {
                    navigator.Update(_currentTargetMovement.position);
                    state.move = navigator.normalizedDeltaPosition * moveSpeed;
                }
                else
                {
                    ChangeTargetPosition(_currentTargetMovement);
                    navigator.Update(_currentTargetMovement.position);
                    state.move = navigator.normalizedDeltaPosition * moveSpeed;
                    Debug.Log("Chase Patrol");
                }
                
            }

            bool NearTarget(Transform target)
            {
                var currentPos2D = new Vector2(transform.position.x, transform.position.z);
                var targetPos2D = new Vector2(target.position.x, target.position.z);
                return Vector2.Distance(currentPos2D, targetPos2D) <= distanceToChangeTarget;
            }

            void ChangeTargetPosition(Transform target)
            {
                if (!target)
                {
                    _currentTargetMovement = GameController.Instance.CurrentMap.GetRandomPatrolTransform;
                }
                else if (NearTarget(_currentTargetMovement))
                {
                    _currentTargetMovement = GameController.Instance.CurrentMap.GetRandomPatrolTransform;
                }
            }
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
            _chase = chaseType != ChaseType.None && enable;
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
            
            //TODO: logic warning to player in FOV
            
            EventGlobalManager.Instance.OnUpdateBloodScreen.Dispatch(true);
            
            //patrol chase
            if (chaseType == ChaseType.Patrol && _chase)
            {
                _chase = false;
            }
        }

        void OnDetectedTarget(GameObject target)
        {
            Debug.Log("Detected Target");
            _currentTarget = target;
            
            // Logic game over
            Debug.LogError("GAME OVER!!!!!!!!");
            
            GameController.Instance.EnemyDetectedPlayer();
        }

        void OnLostTarget(GameObject target)
        {
            Debug.Log("Lost Target");
            _currentTarget = null;
            characterAnimation.SetLookAround();
            
            //TODO: logic disable warning
            
            EventGlobalManager.Instance.OnUpdateBloodScreen.Dispatch(false);

            
            //patrol chase
            if (chaseType == ChaseType.Patrol && !_chase)
            {
                _chase = true;
            }
        }

        #endregion

        void RegisterEvent()
        {
            behaviourPuppet.onRegainBalance.unityEvent.AddListener(StartFOV);
            behaviourPuppet.onRegainBalance.unityEvent.AddListener(StartBehaviour);
            
            behaviourPuppet.onLoseBalance.unityEvent.AddListener(StopFOV);
            behaviourPuppet.onLoseBalance.unityEvent.AddListener(SetFell);

            EventGlobalManager.Instance.OnEnemyKnockout.AddListener(KnockOut);
        }

        void UnregisterEvent()
        {
            behaviourPuppet.onRegainBalance.unityEvent.RemoveListener(StartFOV);
            behaviourPuppet.onRegainBalance.unityEvent.RemoveListener(StartBehaviour);
            
            behaviourPuppet.onLoseBalance.unityEvent.RemoveListener(StopFOV);
            behaviourPuppet.onLoseBalance.unityEvent.RemoveListener(SetFell);
            
            EventGlobalManager.Instance.OnEnemyKnockout.RemoveListener(KnockOut);

        }

        void SetFell()
        {
            GameController.Instance.ImpactCount += 1;
            
            if (_isFell) return;
            _isFell = true;

            GameController.Instance.UpdateEnemyFallStatus();
        }
        
        [Button]
        public void KnockOut()
        {
            behaviourPuppet.Unpin();
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

        public void Fall()
        {
            //TODO: logic enemy fall
            Debug.Log("Enemy Fall");
        }
        
        
        public enum ChaseType
        {
            None,
            Patrol,
            Hunt,
        }
        
    }
}