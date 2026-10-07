using TMPro;
using UnityEngine;

public class AnimatedBeacon : MonoBehaviour
{
    [Header("Animated property")]
    [SerializeField, Range(0f, 1f)] private float brightness;

    [Header("References")]
    [SerializeField] private Light beaconLight;
    [SerializeField] private TMP_Text eventText;

    [Header("Runtime observations")]
    [SerializeField] private int eventCount;

    private void Update()
    {
        beaconLight.intensity = brightness * 5f;
    }

    // Called by a keyframe event on BeaconLoop.anim.
    public void OnPeak()
    {
        eventCount++;
        eventText.text = $"Animation event at the peak\nCalls: {eventCount}";
    }
}
