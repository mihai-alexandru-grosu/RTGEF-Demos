using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EffectTarget : MonoBehaviour
{
    [Header("Effect references")]
    [SerializeField] private Renderer coreRenderer;
    [SerializeField] private AudioSource impactAudio;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem ambientParticles;
    [SerializeField] private DecalProjector scorch;

    [Header("Selection")]
    [SerializeField] private Transform selectionIndicator;

    [Header("Timing")]
    [SerializeField] private float pulseDuration = 1.5f;
    [SerializeField] private float decalDuration = 6f;

    [Header("Runtime observation")]
    [SerializeField] private int activationCount;
    [SerializeField] private float pulse;

    // Each target owns its material so a hit does not change other targets.
    private Material coreMaterial;
    private float decalTime;
    private float selectionPulse;

    public int ActivationCount => activationCount;

    private void Awake()
    {
        coreMaterial = coreRenderer.material;
        ResetEffects();
        SetSelected(false);
    }

    private void Update()
    {
        pulse = Mathf.MoveTowards(pulse, 0f, Time.deltaTime / pulseDuration);
        coreMaterial.SetFloat("_Energy", 0.15f + pulse * 5f);
        decalTime = Mathf.Max(0f, decalTime - Time.deltaTime);
        scorch.fadeFactor = Mathf.Clamp01(decalTime);
        selectionPulse = Mathf.MoveTowards(selectionPulse, 0f, Time.deltaTime * 3f);

        if (selectionIndicator)
            selectionIndicator.localScale = Vector3.one * (1f + Mathf.Sin(selectionPulse * Mathf.PI) * 0.18f);
    }

    // Selection stays visible independently of the four teaching-effect toggles.
    public void SetSelected(bool selected)
    {
        if (!selectionIndicator)
            return;

        selectionIndicator.gameObject.SetActive(selected);
        selectionIndicator.localScale = Vector3.one;
        selectionPulse = selected ? 1f : 0f;
    }

    // Called by the demo controls; flags let the class isolate each lecture topic.
    public void Activate(bool audioEnabled, bool particlesEnabled, bool shaderEnabled, bool decalsEnabled)
    {
        activationCount++;

        if (audioEnabled)
        {
            impactAudio.pitch = Random.Range(0.92f, 1.08f);
            impactAudio.PlayOneShot(impactAudio.clip);
        }

        if (particlesEnabled)
            sparks.Play(true);

        if (shaderEnabled)
            pulse = 1f;

        if (decalsEnabled)
        {
            decalTime = decalDuration;
            scorch.fadeFactor = 1f;
        }
    }

    public void SetSpatialAudio(bool spatial)
    {
        impactAudio.spatialBlend = spatial ? 1f : 0f;
    }

    public void SetAmbientParticles(bool enabled)
    {
        if (enabled)
            ambientParticles.Play();
        else
            ambientParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void ResetEffects()
    {
        activationCount = 0;
        pulse = 0f;
        decalTime = 0f;
        scorch.fadeFactor = 0f;
        impactAudio.Stop();
        sparks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        coreMaterial.SetFloat("_Energy", 0.15f);
    }

    private void OnDestroy()
    {
        if (coreMaterial)
            Destroy(coreMaterial);
    }
}
