using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController[] controllers;
    [SerializeField] private TMP_Text statusText;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1.6f;
    [SerializeField] private float runSpeed = 4f;
    [Tooltip("Speed increase per second when starting to move or sprint.")]
    [SerializeField, Min(0.01f)] private float acceleration = 4f;
    [Tooltip("Speed decrease per second when slowing down or stopping.")]
    [SerializeField, Min(0.01f)] private float deceleration = 6f;
    [Tooltip("Turning speed in degrees per second.")]
    [SerializeField, Min(0f)] private float rotationSpeed = 540f;
    [Tooltip("Damping time in seconds for directional animation changes.")]
    [SerializeField, Min(0f)] private float directionBlendTime = 0.15f;
    [SerializeField] private Vector2 areaSize = new Vector2(5f, 3f);

    [Header("Runtime observations")]
    [SerializeField] private int controllerIndex;
    [SerializeField] private bool additiveEnabled;

    // Runtime state.
    private InputAction moveAction;
    private InputAction sprintAction;
    private Vector3 home;
    private float currentSpeed;
    private Vector3 movementDirection;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        sprintAction = InputSystem.actions.FindAction("Player/Sprint");
        home = transform.position;
        SetController(0);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.digit1Key.wasPressedThisFrame)
                SetController(0);
            if (keyboard.digit2Key.wasPressedThisFrame)
                SetController(1);
            if (keyboard.digit3Key.wasPressedThisFrame)
                SetController(2);
            if (keyboard.eKey.wasPressedThisFrame)
                animator.SetTrigger("Wave");
            if (keyboard.qKey.wasPressedThisFrame)
                additiveEnabled = !additiveEnabled;
        }

        Vector2 input = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
        bool sprinting = sprintAction.IsPressed();
        float targetSpeed = input.magnitude * (sprinting ? runSpeed : walkSpeed);
        float speedChange = targetSpeed > currentSpeed ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChange * Time.deltaTime);

        // Retain the last direction while slowing to a stop.
        if (input.sqrMagnitude > 0f)
            movementDirection = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 next = transform.position + movementDirection * currentSpeed * Time.deltaTime;
        next.x = Mathf.Clamp(next.x, home.x - areaSize.x, home.x + areaSize.x);
        next.z = Mathf.Clamp(next.z, home.z - areaSize.y, home.z + areaSize.y);
        transform.position = next;

        // Directional blending keeps the body facing forward so strafing stays visible.
        if (controllerIndex != 2 && currentSpeed > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        animator.SetBool("isWalking", currentSpeed > 0.001f);
        
        // Movement and the blend trees share the accelerating speed.
        animator.SetFloat("walkSpeed", currentSpeed);
        Vector2 blendDirection = new Vector2(movementDirection.x, movementDirection.z) * (currentSpeed / walkSpeed);
        animator.SetFloat("lateralSpeed", blendDirection.x, directionBlendTime, Time.deltaTime);
        animator.SetFloat("forwardSpeed", blendDirection.y, directionBlendTime, Time.deltaTime);
        animator.SetLayerWeight(2, additiveEnabled ? 1f : 0f);

        string mode = controllerIndex == 0 ? "Idle / Walking" : controllerIndex == 1 ? "1D speed blend" : "2D directional blend";
        statusText.text = $"{mode}\nwalkSpeed: {currentSpeed:0.00}\nforwardSpeed: {animator.GetFloat("forwardSpeed"):0.00}   lateralSpeed: {animator.GetFloat("lateralSpeed"):0.00}\nAdditive breathing: {(additiveEnabled ? "ON" : "OFF")}";
    }

    public void SetController(int index)
    {
        controllerIndex = index;
        currentSpeed = 0f;
        movementDirection = Vector3.zero;
        animator.runtimeAnimatorController = controllers[index];
        animator.Rebind();
        transform.rotation = Quaternion.identity;
    }
}
