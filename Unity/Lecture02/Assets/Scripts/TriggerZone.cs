using TMPro;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text statusText;

    [Header("Diagnostics")]
    [SerializeField] private bool logEvents;

    [Header("Runtime state")]
    [SerializeField] private int staySteps;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        staySteps = 0;
        statusText.text = "Inside the green zone: triggers detect you without blocking movement.";

        if (logEvents)
            Debug.Log("Trigger enter: Player", this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            staySteps++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        statusText.text = "Left the trigger zone. Collect the gold coins!";

        if (logEvents)
            Debug.Log($"Trigger exit after {staySteps} physics steps", this);
    }
}

