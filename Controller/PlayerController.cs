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
        
        public Transform player;
        [SerializeField] float groundYOffset;
        [SerializeField] LayerMask groundMask;
        Vector3 spherePos;
        [SerializeField] float gravity = -9.81f;
        Vector3 velocity;
        [Header("Config")]
        public bool lookWithMovement;
        [Header("Animation Variables")]
        public string walkAnimParam = "isWalking";

        CharacterController controller;
        Animator animator;
        #region Input Variables
        float hzInput, vInput;
        private PlayerInput playerInput;
        private InputAction moveAction;

        bool _isMoving = false;
        public bool isMoving
        {
            get { return _isMoving; }
            set { _isMoving = value;
                // Setting movement animation
                animator?.SetBool(walkAnimParam, value);
            }
        }
        #endregion

        #region MonoBehaviour Lifecycle
        // Start is called before the first frame update
        public virtual void Start()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                moveAction = playerInput.actions["Move"];
            }
            #region Grabbbing Animator
            if (player != null)
            {
                animator = player.GetComponent<Animator>();
            }
            if(animator == null)
            {
                animator = GetComponent<Animator>();
            }
            #endregion
        }

        // Update is called once per frame
        public virtual void Update()
        {
            GetDirectionAndMove();
            Gravity();
        }
        #endregion
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
                if(hzInput!=0 || vInput!=0)
                {
                    if(lookWithMovement)
                    {
                        Vector3 lookDir = transform.forward * vInput + transform.right * hzInput;
                        if(player == null)
                        {
                            transform.rotation = Quaternion.LookRotation(lookDir);
                        }
                        else
                        {
                            player.rotation = Quaternion.LookRotation(lookDir);
                        }
                    }
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                }
            }
            else
            {
                hzInput = 0;
                vInput = 0;
                isMoving = false;
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
