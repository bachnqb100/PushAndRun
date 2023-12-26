using System;
using CnControls;
using DefaultNamespace;
using RootMotion.Demos;
using UnityEngine;

namespace Player
{
    public class UserControlPlayer : UserControlThirdPerson
    {

        private bool _jump;

        private void OnEnable()
        {
            _jump = false;

            EventGlobalManager.Instance.OnPlayerJump.AddListener(SetJump);
        }

        private void OnDisable()
        {
            EventGlobalManager.Instance.OnPlayerJump.RemoveListener(SetJump);
        }

        protected override void Update () {
            // read inputs
            state.crouch = canCrouch && Input.GetKey(KeyCode.C);
            state.jump = canJump && _jump;

            float h = CnInputManager.GetAxisRaw("Horizontal");
            float v = CnInputManager.GetAxisRaw("Vertical");
			
            // calculate move direction
            Vector3 move = cam.rotation * new Vector3(h, 0f, v).normalized;

            // Flatten move vector to the character.up plane
            if (move != Vector3.zero) {
                Vector3 normal = transform.up;
                Vector3.OrthoNormalize(ref normal, ref move);
                state.move = move;
            } else state.move = Vector3.zero;

            bool walkToggle = Input.GetKey(KeyCode.LeftShift);

            // We select appropriate speed based on whether we're walking by default, and whether the walk/run toggle button is pressed:
            float walkMultiplier = (walkByDefault ? walkToggle ? 1 : 0.5f : walkToggle ? 0.5f : 1);

            state.move *= walkMultiplier;
			
            // calculate the head look target position
            state.lookPos = transform.position + cam.forward * 100f;
        }

        void SetJump(bool jump) => _jump = jump;

    }
}