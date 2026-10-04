using UnityEngine;

namespace Lecture03
{
    // Alternative to InputActionReference. Enable this example in TileWorkshop.
    public class GeneratedInputExample : MonoBehaviour
    {
        [Header("Runtime observation")]
        [SerializeField] private float horizontal;
        [SerializeField] private Vector2 aim;
        [SerializeField] private Vector2 pointer;

        // Runtime state.
        private LectureInput input;

        private void Awake()
        {
            input = new LectureInput();
        }

        private void OnEnable()
        {
            input.Player.Enable();
        }

        private void OnDisable()
        {
            input.Player.Disable();
        }

        private void Update()
        {
            horizontal = input.Player.Move.ReadValue<float>();
            aim = input.Player.Aim.ReadValue<Vector2>();
            pointer = input.Player.Pointer.ReadValue<Vector2>();
        }

        private void OnDestroy()
        {
            input.Dispose();
        }
    }
}
