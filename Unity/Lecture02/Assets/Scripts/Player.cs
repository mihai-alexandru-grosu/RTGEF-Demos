using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpImpulse = 6f;

    [Header("Runtime state")]
    [SerializeField] private Rigidbody rb;
    
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector3 movement;
    private bool jumpRequested;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        moveAction = InputSystem.actions.FindAction("Player/Move", true);
        jumpAction = InputSystem.actions.FindAction("Player/Jump", true);
    }

    private void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0f, input.y).normalized;

        if (jumpAction.WasPressedThisFrame())
            jumpRequested = true;
    }

    private void FixedUpdate()
    {
        // Velocity is in metres per second, so no deltaTime is needed here.
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
        bool grounded = Physics.Raycast(rb.position, Vector3.down, 0.58f);

        if (jumpRequested && grounded && rb.linearVelocity.y <= 0.1f)
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);

        jumpRequested = false;
    }
}
