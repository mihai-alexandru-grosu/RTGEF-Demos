using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class EffectsDemo : MonoBehaviour
{
    [Header("Scene references")]
    [SerializeField] private EffectTarget[] targets;
    [SerializeField] private Transform listenerCamera;
    [SerializeField] private AudioSource ambience;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private UnityEngine.UI.Text statusText;
    [SerializeField] private InputActionAsset controls;

    [Header("Camera movement")]
    [SerializeField] private float movementSpeed = 6f;

    [Header("Runtime observation")]
    [SerializeField] private int selectedTarget = 1;
    [SerializeField] private bool audioEnabled = true;
    [SerializeField] private bool particlesEnabled = true;
    [SerializeField] private bool shaderEnabled = true;
    [SerializeField] private bool decalsEnabled = true;
    [SerializeField] private bool spatialAudio = true;
    [SerializeField] private float sfxVolume = 0.8f;
    [SerializeField] private float ambienceVolume = 0.35f;

    private InputActionMap demoMap;
    private InputAction moveAction;
    private InputAction activateAction;
    private InputAction resetAction;
    private InputAction[] selectActions;
    private InputAction[] toggleActions;
    private InputAction sfxDownAction;
    private InputAction sfxUpAction;
    private InputAction ambienceDownAction;
    private InputAction ambienceUpAction;
    private Vector3 initialCameraPosition;

    private void Awake()
    {
        initialCameraPosition = listenerCamera.position;
        demoMap = controls.FindActionMap("Demo", true);
        moveAction = demoMap.FindAction("Move", true);
        activateAction = demoMap.FindAction("Activate", true);
        resetAction = demoMap.FindAction("Reset", true);
        selectActions = new InputAction[targets.Length];

        for (int i = 0; i < selectActions.Length; i++)
            selectActions[i] = demoMap.FindAction("Target" + (i + 1), true);

        string[] names =
        {
            "Audio", "Particles", "Shader", "Decals", "Spatial", "Ambience"
        };
        toggleActions = new InputAction[names.Length];

        for (int i = 0; i < names.Length; i++)
            toggleActions[i] = demoMap.FindAction(names[i], true);

        sfxDownAction = demoMap.FindAction("SfxDown", true);
        sfxUpAction = demoMap.FindAction("SfxUp", true);
        ambienceDownAction = demoMap.FindAction("AmbienceDown", true);
        ambienceUpAction = demoMap.FindAction("AmbienceUp", true);
    }

    private void OnEnable()
    {
        demoMap.Enable();
    }

    private void Start()
    {
        ResetDemo();
    }

    private void Update()
    {
        if (resetAction.WasPressedThisFrame())
            ResetDemo();

        for (int i = 0; i < selectActions.Length; i++)
        {
            if (selectActions[i].WasPressedThisFrame())
                SelectTarget(i);
        }

        if (toggleActions[0].WasPressedThisFrame())
            audioEnabled = !audioEnabled;
        if (toggleActions[1].WasPressedThisFrame())
        {
            particlesEnabled = !particlesEnabled;

            foreach (EffectTarget target in targets)
                target.SetAmbientParticles(particlesEnabled);
        }
        if (toggleActions[2].WasPressedThisFrame())
            shaderEnabled = !shaderEnabled;
        if (toggleActions[3].WasPressedThisFrame())
            decalsEnabled = !decalsEnabled;
        if (toggleActions[4].WasPressedThisFrame())
        {
            spatialAudio = !spatialAudio;

            foreach (EffectTarget target in targets)
                target.SetSpatialAudio(spatialAudio);
        }
        if (toggleActions[5].WasPressedThisFrame())
            ambience.mute = !ambience.mute;

        if (sfxDownAction.WasPressedThisFrame())
            sfxVolume = Mathf.Max(0f, sfxVolume - 0.1f);
        if (sfxUpAction.WasPressedThisFrame())
            sfxVolume = Mathf.Min(1f, sfxVolume + 0.1f);
        if (ambienceDownAction.WasPressedThisFrame())
            ambienceVolume = Mathf.Max(0f, ambienceVolume - 0.1f);
        if (ambienceUpAction.WasPressedThisFrame())
            ambienceVolume = Mathf.Min(1f, ambienceVolume + 0.1f);

        SetMixerVolume("SfxVolume", sfxVolume);
        SetMixerVolume("AmbienceVolume", ambienceVolume);

        Vector2 movement = moveAction.ReadValue<Vector2>();
        Vector3 position = listenerCamera.position;
        position += new Vector3(movement.x, 0f, movement.y) * movementSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, -10f, 10f);
        position.z = Mathf.Clamp(position.z, -24f, -6f);
        listenerCamera.position = position;
        listenerCamera.LookAt(new Vector3(0f, 1f, 1f));

        if (activateAction.WasPressedThisFrame())
            targets[selectedTarget].Activate(audioEnabled, particlesEnabled, shaderEnabled, decalsEnabled);

        statusText.text = $"TARGET {selectedTarget + 1}    Activations: {targets[selectedTarget].ActivationCount}\n"
            + $"A  Audio: {State(audioEnabled)}     P  Particles: {State(particlesEnabled)}     G  Material: {State(shaderEnabled)}     D  Decal: {State(decalsEnabled)}\n"
            + $"T  Sound: {(spatialAudio ? "3D spatial" : "2D global")}     SFX: {sfxVolume:P0}     Ambience: {(ambience.mute ? "muted" : ambienceVolume.ToString("P0"))}";
    }

    private void SetMixerVolume(string parameter, float linearVolume)
    {
        // Mixer attenuation uses decibels; log(0) is undefined, so silence is -80 dB.
        float decibels = linearVolume > 0.0001f ? 20f * Mathf.Log10(linearVolume) : -80f;
        mixer.SetFloat(parameter, decibels);
    }

    private string State(bool enabled)
    {
        return enabled ? "on" : "off";
    }

    private void ResetDemo()
    {
        selectedTarget = 1;
        audioEnabled = true;
        particlesEnabled = true;
        shaderEnabled = true;
        decalsEnabled = true;
        spatialAudio = true;
        sfxVolume = 0.8f;
        ambienceVolume = 0.35f;
        listenerCamera.position = initialCameraPosition;
        ambience.mute = false;
        ambience.Stop();
        ambience.Play();

        foreach (EffectTarget target in targets)
        {
            target.ResetEffects();
            target.SetSpatialAudio(true);
            target.SetAmbientParticles(true);
        }

        SelectTarget(selectedTarget);

        SetMixerVolume("SfxVolume", sfxVolume);
        SetMixerVolume("AmbienceVolume", ambienceVolume);
    }

    private void OnDisable()
    {
        demoMap.Disable();
    }

    private void SelectTarget(int index)
    {
        selectedTarget = index;

        for (int i = 0; i < targets.Length; i++)
            targets[i].SetSelected(i == selectedTarget);
    }
}
