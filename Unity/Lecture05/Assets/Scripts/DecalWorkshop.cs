using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class DecalWorkshop : MonoBehaviour
{
    [SerializeField] private DecalProjector movingProjector;
    [SerializeField] private GameObject[] marks;
    [SerializeField] private GameObject opaqueReceiver;
    [SerializeField] private UnityEngine.UI.Text status;
    [SerializeField] private InputActionAsset controls;

    private InputActionMap actionMap;
    private InputAction move;
    private InputAction toggle;
    private InputAction reset;
    private InputAction shrink;
    private InputAction grow;
    private InputAction toggleWall;
    private Vector3 initialPosition;
    private Vector3 initialSize;
    private bool visible = true;

    private void Awake()
    {
        initialPosition = movingProjector.transform.localPosition;
        initialSize = movingProjector.size;
        actionMap = controls.FindActionMap("Decals", true);
        move = actionMap.FindAction("Move", true);
        toggle = actionMap.FindAction("Toggle", true);
        reset = actionMap.FindAction("Reset", true);
        shrink = actionMap.FindAction("Shrink", true);
        grow = actionMap.FindAction("Grow", true);
        toggleWall = actionMap.FindAction("ToggleWall", true);
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void Update()
    {
        Vector2 input = move.ReadValue<Vector2>();
        Vector3 position = movingProjector.transform.localPosition;
        position += new Vector3(input.x, input.y, 0f) * Time.deltaTime * 2f;
        position.x = Mathf.Clamp(position.x, -1.2f, 1.2f);
        position.y = Mathf.Clamp(position.y, 0.5f, 3.5f);
        movingProjector.transform.localPosition = position;
        float change = (grow.IsPressed() ? 1f : 0f) - (shrink.IsPressed() ? 1f : 0f);
        float size = Mathf.Clamp(movingProjector.size.x + change * Time.deltaTime * 2f, 0.7f, 4f);
        movingProjector.size = new Vector3(size, size, initialSize.z);

        if (toggle.WasPressedThisFrame())
            visible = !visible;
        if (toggleWall.WasPressedThisFrame())
            opaqueReceiver.SetActive(!opaqueReceiver.activeSelf);
        if (reset.WasPressedThisFrame())
        {
            visible = true;
            opaqueReceiver.SetActive(true);
            movingProjector.transform.localPosition = initialPosition;
            movingProjector.size = initialSize;
        }

        foreach (GameObject mark in marks)
            mark.SetActive(visible);

        status.text = $"Decals {(visible ? "ON" : "OFF")}    Moving projector width: {movingProjector.size.x:F1}    Wall behind glass: {(opaqueReceiver.activeSelf ? "ON" : "OFF")}\nARROWS  Move    - / =  Resize    D  Decals on/off    B  Wall behind glass    R  Reset\nF1  Effects + audio    F2  Particles    F3  Decals    F4  Shader Graph";
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }
}
