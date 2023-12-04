using System;
using CnControls;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Title("Component")] 
        [SerializeField] private CharacterController characterController;
        [SerializeField] private AnimationController animationController;

        
        [Title("Stats")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float turnSmoothTime = 0.1f;

        [Header("Animation")] 
        [SerializeField] private float jumpAnimationDuration = 1f;

        [Header("Test")] 
        [SerializeField] private Transform destination;
        


        private Vector3 _moveDir;
        private bool _isMoving;
        private bool _isJumping;
        private float _turnSmoothVelocity;

        private void Start()
        {
            _isMoving = false;
            _isJumping = false;
            
            Run();
        }

        private void Update()
        {
            Move();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        void Move()
        {
            float horizontal = - CnInputManager.GetAxis("Horizontal");
            float vertical = - CnInputManager.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            
            animationController.SetMoveSpeedAnim(direction.sqrMagnitude);

            if (direction.sqrMagnitude >= 0.01f)
            {
                float targetAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg +
                                    Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back;
                characterController.SimpleMove(moveDir.normalized * moveSpeed * Time.deltaTime);
            }
        }
        
        [Button]
        void Jump()
        {
            _isMoving = false;
            _isJumping = true;
            animationController.Jump();
            
            characterController.enabled = false;

            DOVirtual.DelayedCall(jumpAnimationDuration, () => Run());
        }

        [Button]
        void Run()
        {
            characterController.enabled = true;
            _isMoving = !_isMoving;
        }
    }
}