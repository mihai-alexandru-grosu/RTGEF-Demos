using TMPro;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Renderer indicator;

    [Header("Runtime observations")]
    [SerializeField] private int footstepCount;
    [SerializeField] private int waveCount;

    // Runtime state.
    private float flashTime;

    private void Update()
    {
        flashTime = Mathf.Max(0f, flashTime - Time.deltaTime);
        indicator.material.color = flashTime > 0f ? Color.yellow : new Color(0.1f, 0.4f, 0.6f);
    }

    // Events on the editable walking/running clips pass the foot index.
    public void OnFootstep(int foot)
    {
        footstepCount++;
        flashTime = 0.12f;
        audioSource.pitch = foot == 0 ? 0.9f : 1.1f;
        audioSource.Play();
        eventText.text = $"Footstep events: {footstepCount}\nLast foot: {(foot == 0 ? "left" : "right")}\nCompleted waves: {waveCount}";
    }

    public void OnWaveFinished()
    {
        waveCount++;
        eventText.text = $"Footstep events: {footstepCount}\nCompleted waves: {waveCount}";
    }
}
