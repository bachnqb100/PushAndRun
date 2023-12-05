using System;
using System.Collections;
using RootMotion.Demos;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class EnemyPuppet : MonoBehaviour
    {
        [Title("Components")] 
        [SerializeField] private CharacterAnimationBase characterAnimation;

        [SerializeField] private Rigidbody rb;

        [Title("Stats")] 
        [SerializeField] private bool smoothPhysics;
        
        
        [Header("Jumping and Falling")]
        public bool smoothJump = true; // If true, adds jump force over a few fixed time steps, not in a single step
        public float airSpeed = 6f; // determines the max speed of the character while airborne
        public float jumpPower = 12f; // determines the jump force applied when jumping (and therefore the jump height)

        
        private Animator _animator;
        private bool _onGround;

        private void Start()
        {
            if (_animator == null)
                _animator = characterAnimation.GetComponent<Animator>();
            
            characterAnimation.smoothFollow = smoothPhysics;
            _onGround = true;
        }

        void Jump()
        {
            if (!characterAnimation.animationGrounded) return;
            
            // Jump
            _onGround = false;
            //jumpEndTime = Time.time + 0.1f;

            Vector3 jumpVelocity = transform.forward * airSpeed;
            jumpVelocity += transform.up * jumpPower;

            if (smoothJump)
            {
                StopAllCoroutines();
                StartCoroutine(JumpSmooth(jumpVelocity - rb.velocity));
            } else
            {
                rb.velocity = jumpVelocity;
            }
        }
        
        IEnumerator JumpSmooth(Vector3 jumpVelocity)
        {
            int steps = 0;
            int stepsToTake = 3;
            while (steps < stepsToTake)
            {
                rb.AddForce((jumpVelocity) / stepsToTake, ForceMode.VelocityChange);
                steps++;
                yield return new WaitForFixedUpdate();
            }
        }
        
        
    }
    
    
    
  
}