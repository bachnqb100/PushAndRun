using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class AnimationController : MonoBehaviour
    {
        [Title("Components")]
        [SerializeField] private Animator animator;

        [Title("Properties")]
        [SerializeField] private float dampTimeAnimation = 0.1f;
        [SerializeField] private float deltaTimeAnimation = 0.1f;

        public void SetMoveSpeedAnim(float speed)
        {
            animator.SetFloat(GameConst.MoveSpeed, speed, dampTimeAnimation, deltaTimeAnimation);
        }

        public void Jump()
        {
            animator.SetTrigger(GameConst.Jump);
        }

        public void Jump(string nameAnimJump)
        {
            animator.SetTrigger(nameAnimJump);
        }

        public void SetStatusRootMotion(bool enable = true)
        {
            animator.applyRootMotion = enabled;
        } 
    }
}