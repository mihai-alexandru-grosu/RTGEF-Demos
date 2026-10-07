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
    [SerializeField] private Vector2 areaSize = new Vector2(5f, 3f);

    [Header("Runtime observations")]
    [SerializeField] private int controllerIndex;
    [SerializeField] private bool additiveEnabled;

    // Runtime state.
    private InputAction moveAction;
    private InputAction sprintAction;
    private Vector3 home;

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
        float speed = sprinting ? runSpeed : walkSpeed;
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        Vector3 next = transform.position + direction * speed * Time.deltaTime;
        next.x = Mathf.Clamp(next.x, home.x - areaSize.x, home.x + areaSize.x);
        next.z = Mathf.Clamp(next.z, home.z - areaSize.y, home.z + areaSize.y);
        transform.position = next;

        // Directional blending keeps the body facing forward so strafing stays visible.
        if (controllerIndex != 2 && direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);

        animator.SetBool("isWalking", input.sqrMagnitude > 0.001f);
        animator.SetFloat("walkSpeed", input.magnitude * speed);
        Vector2 blendDirection = input * (sprinting ? runSpeed / walkSpeed : 1f);
        animator.SetFloat("lateralSpeed", blendDirection.x);
        animator.SetFloat("forwardSpeed", blendDirection.y);
        animator.SetLayerWeight(2, additiveEnabled ? 1f : 0f);

        string mode = controllerIndex == 0 ? "Idle / Walking" : controllerIndex == 1 ? "1D speed blend" : "2D directional blend";
        statusText.text = $"{mode}\nwalkSpeed: {input.magnitude * speed:0.00}\nforwardSpeed: {blendDirection.y:0.00}   lateralSpeed: {blendDirection.x:0.00}\nAdditive breathing: {(additiveEnabled ? "ON" : "OFF")}";
    }

    public void SetController(int index)
    {
        controllerIndex = index;
        animator.runtimeAnimatorController = controllers[index];
        animator.Rebind();
        transform.rotation = Quaternion.identity;
    }
}
