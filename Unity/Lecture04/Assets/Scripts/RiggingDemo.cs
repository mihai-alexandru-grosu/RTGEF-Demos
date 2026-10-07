using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class RiggingDemo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rig rig;
    [SerializeField] private Transform handTarget;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private TMP_Text statusText;

    [Header("Runtime observations")]
    [SerializeField] private bool rigEnabled = true;
    [SerializeField] private bool movingTargets = true;

    // Runtime state.
    private Vector3 handHome;
    private Vector3 lookHome;
    private float elapsed;

    private void Start()
    {
        handHome = handTarget.localPosition;
        lookHome = lookTarget.localPosition;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.iKey.wasPressedThisFrame)
                rigEnabled = !rigEnabled;
            if (keyboard.tKey.wasPressedThisFrame)
                movingTargets = !movingTargets;
        }

        rig.weight = rigEnabled ? 1f : 0f;

        if (movingTargets)
        {
            elapsed += Time.deltaTime;
            handTarget.localPosition = handHome + new Vector3(Mathf.Sin(elapsed) * 0.1f, Mathf.Cos(elapsed) * 0.1f, 0f);
            lookTarget.localPosition = lookHome + new Vector3(Mathf.Sin(elapsed * 0.8f) * 1.1f, 0f, 0f);
        }

        statusText.text = $"Rig weight: {rig.weight:0}\nMoving targets: {(movingTargets ? "ON" : "OFF")}\nOrange: right hand IK\nBlue: head aim\nGreen: second aim source";
    }
}
