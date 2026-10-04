using UnityEngine;
using UnityEngine.InputSystem;

namespace Lecture03
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputExample : MonoBehaviour
    {
        [Header("Runtime observation")]
        [SerializeField] private float horizontal;
        [SerializeField] private string lastAction;

        // Runtime state.
        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            playerInput.onActionTriggered += OnAction;
        }

        private void OnDisable()
        {
            playerInput.onActionTriggered -= OnAction;
        }

        private void OnAction(InputAction.CallbackContext context)
        {
            lastAction = context.action.name + ": " + context.phase;

            if (context.action.name == "Move")
                horizontal = context.ReadValue<float>();
        }
    }
}
