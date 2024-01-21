using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    public class CharacterMovement : CharacterAbility
    {
        public float speed = 5f;
        public float groundCheckDistance = 0.4f;
        public LayerMask groundMask;
        public bool isGrounded;
        public Transform groundCheckCenter;
        public float jumpHeight = 2f;
        public float gravity = -20f;
        public float MaximumFallSpeed = 20.0f;
        public Vector3 velocity;

        private Vector3 _lastFrameVelocity;
        protected CharacterState.CharacterMoveState _currentMoveState;

        protected override void Init()
        {
            base.Init();
            ResetVariables();
        }

        public override void RunAbility()
        {
            base.RunAbility();
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            velocity = new Vector3(horizontal, 0, vertical) * speed;
            velocity.y = _lastFrameVelocity.y;
            velocity = transform.TransformDirection(velocity);
            //_characterController.Move(movement);
            CheckGround();
            if (isGrounded)
            {
                velocity.y = 0f;
                _currentMoveState = CharacterState.CharacterMoveState.Idle;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
                velocity.y = Mathf.Max(velocity.y, -MaximumFallSpeed);
                _currentMoveState = CharacterState.CharacterMoveState.Falling;
            }
            if (Input.GetKeyDown("space"))
            {
                if (isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            _characterController.Move(velocity * Time.deltaTime);
            _lastFrameVelocity = velocity;
        }

        public void CheckGround()
        {
            isGrounded = Physics.CheckSphere(groundCheckCenter.position, groundCheckDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f;
            }
        }
        private void ResetVariables()
        {
            velocity = Vector3.zero;
            _lastFrameVelocity = Vector3.zero;
            isGrounded = false;
            _currentMoveState = CharacterState.CharacterMoveState.Idle;
        }

        void OnDrawGizmos()
        {
            // Debug purpose, show the groundCheck Sphere
            // Check if groundCheckCenter is assigned to avoid null reference errors
            if (groundCheckCenter != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheckCenter.position, groundCheckDistance);
            }
        }
    }
}
