using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace MMARGame.Controller
{
    [RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 3;
        public Vector3 dir;
        float hzInput, vInput;
        CharacterController controller;
        public Transform player;
        [SerializeField] float groundYOffset;
        [SerializeField] LayerMask groundMask;
        Vector3 spherePos;
        [SerializeField] float gravity = -9.81f;
        Vector3 velocity;
        private PlayerInput playerInput;
        private InputAction moveAction;
        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                moveAction = playerInput.actions["Move"];
            }
        }

        // Update is called once per frame
        void Update()
        {
            GetDirectionAndMove();
            Gravity();
        }
        public void AimAt(Vector3 position)
        {
            position.y=player.position.y;
            player.LookAt(position);
        }
        void GetDirectionAndMove()
        {
            if (moveAction != null)
            {
                Vector2 input = moveAction.ReadValue<Vector2>();
                hzInput = input.x;
                vInput = input.y;
            }
            else
            {
                hzInput = 0;
                vInput = 0;
            }
            dir = transform.forward * vInput + transform.right * hzInput;
            controller.Move(dir.normalized * moveSpeed * Time.deltaTime);
        }
        bool IsGrounded()
        {
            spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
            return Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);
        }

        void Gravity()
        {
            if (!IsGrounded())
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else if (velocity.y < 0)
            {
                velocity.y = -2;
            }
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
