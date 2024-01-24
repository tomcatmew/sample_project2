using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    /// <summary>
    /// The main movement class which will control the character movement and jump.
    /// This class is derived from Character ability class.
    /// W S A D  control the movement , SPACE control the jump, Mouse to rotate character
    /// </summary>
    public class CharacterMovement : CharacterAbility
    {
        public float speed = 5f;
        public float groundCheckDistance = 0.42f;
        public LayerMask groundMask;
        public bool isGrounded;
        public Transform groundCheckCenter;
        public float jumpHeight = 2f;
        public float gravity = -25f;
        public float MaximumFallSpeed = 20.0f;
        // Set velocity as public so we can see from Editor
        public Vector3 velocity;
        public float rotationSpeed = 1f;
        public float groundCheckDelay = 0.2f;


        private Vector3 _lastFrameVelocity;
        private float _lastJumpTime;
        protected CharacterState.CharacterMoveState _currentMoveState;

        protected override void Init()
        {
            base.Init();
            ResetVariables();
        }

        public override void RunAbility()
        {
            base.RunAbility();
            // Get user's input, W S A D for movement, X axis mouse movment to rotate character
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float mouseHorizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.Rotate(0, mouseHorizontal, 0);
            velocity = new Vector3(horizontal, 0, vertical) * speed;
            velocity.y = _lastFrameVelocity.y;
            velocity = transform.TransformDirection(velocity);
            _currentMoveState = CharacterState.CharacterMoveState.Walk;

            CheckGround();
            if (isGrounded)
            {
                // Give it a small downward velocity to make sure it always touch the ground
                // e.g standing on the slopes.
                velocity.y = -2f;
                _currentMoveState = CharacterState.CharacterMoveState.Idle;
            }
            else
            {
                // Apply gravity in the air
                velocity.y += gravity * Time.deltaTime;
                // Limit the max falling speed to prevent falling too fast
                velocity.y = Mathf.Max(velocity.y, -MaximumFallSpeed);
                _currentMoveState = CharacterState.CharacterMoveState.Falling;
            }
            if (Input.GetKeyDown("space"))
            {
                if (isGrounded)
                {
                    // Compute the initial velocity which needs to reach the jump height, v = sqrt(2gh)
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    _lastJumpTime = Time.time;
                    _currentMoveState = CharacterState.CharacterMoveState.Jump;
                }
            }
            // Update the character position
            _characterController.Move(velocity * Time.deltaTime);
            _lastFrameVelocity = velocity;
        }

        public void CheckGround()
        {
            // Disable ground check when Jump button is clicked to prevent character is stopped by nearby obstacles
            if (Time.time < _lastJumpTime + groundCheckDelay)
            {
                isGrounded = false;
            }
            else
            {
                isGrounded = Physics.CheckSphere(groundCheckCenter.position, groundCheckDistance, groundMask);
            }
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

            if(groundCheckCenter == null)
            {
                groundCheckCenter = GameObject.Find("GoundCheckCenterer").transform;
                if (groundCheckCenter == null)
                    Debug.LogError("Cannot find 'GroundCheckCenterer' game object in the scene");
            }
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
