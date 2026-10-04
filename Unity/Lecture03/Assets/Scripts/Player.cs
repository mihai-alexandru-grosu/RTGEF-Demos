using UnityEngine;
using UnityEngine.InputSystem;

namespace Lecture03
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Input references")]
        [SerializeField] private InputActionReference move;
        [SerializeField] private InputActionReference jump;

        [Header("Movement")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float jumpSpeed = 11f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private float groundRadius = 0.12f;

        [Header("Visuals")]
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;

        [Header("Runtime observation")]
        [SerializeField] private bool grounded;
        [SerializeField] private float horizontal;

        // Runtime state.
        private Rigidbody2D body;
        private Vector2 spawn;
        private bool jumpRequested;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            spawn = body.position;
        }

        private void OnEnable()
        {
            move.action.Enable();
            jump.action.performed += OnJump;
            jump.action.Enable();
        }

        private void OnDisable()
        {
            jump.action.performed -= OnJump;
            move.action.Disable();
            jump.action.Disable();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            jumpRequested = true;
        }

        private void Update()
        {
            horizontal = move.action.ReadValue<float>();

            if (Mathf.Abs(horizontal) > 0.01f)
                sprite.flipX = horizontal < 0f;

            animator.SetFloat("Speed", Mathf.Abs(horizontal));
            animator.SetFloat("VerticalSpeed", body.linearVelocity.y);
            animator.SetBool("Grounded", grounded);

            if (body.position.y < -8f)
                Respawn();
        }

        private void FixedUpdate()
        {
            Collider2D ground = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayers);
            grounded = ground;
            bool onConveyor = ground && ground.GetComponent<SurfaceEffector2D>();

            // Let the conveyor carry an idle player instead of resetting its velocity.
            if (!onConveyor || Mathf.Abs(horizontal) > 0.01f)
                body.linearVelocity = new Vector2(horizontal * speed, body.linearVelocity.y);

            if (jumpRequested && grounded)
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);

            jumpRequested = false;
        }

        public void Respawn()
        {
            body.position = spawn;
            body.linearVelocity = Vector2.zero;
            jumpRequested = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (!groundCheck)
                return;

            Gizmos.color = grounded ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
