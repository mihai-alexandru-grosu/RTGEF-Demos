using UnityEngine;
using UnityEngine.InputSystem;

public class ParticleWorkshop : MonoBehaviour
{
    [Header("Gallery")]
    [SerializeField] private Transform[] stations;
    [SerializeField] private ParticleSystem[] emitters;
    [SerializeField] private Camera galleryCamera;
    [SerializeField] private UnityEngine.UI.Text status;
    [SerializeField] private InputActionAsset controls;

    private ParticleSystem[] allSystems;
    private ParticleMotion[] motions;
    private ParticleCollisionCounter[] counters;
    private InputActionMap actionMap;
    private InputAction replay;
    private InputAction pause;
    private InputAction overview;
    private InputAction[] focus;
    private Vector3 cameraStart;
    private float cameraSize;
    private bool paused;
    private int selected = -1;

    private void Awake()
    {
        allSystems = GetComponentsInChildren<ParticleSystem>();
        motions = GetComponentsInChildren<ParticleMotion>();
        counters = GetComponentsInChildren<ParticleCollisionCounter>();
        cameraStart = galleryCamera.transform.position;
        cameraSize = galleryCamera.orthographicSize;
        actionMap = controls.FindActionMap("Workshop", true);
        replay = actionMap.FindAction("Replay", true);
        pause = actionMap.FindAction("Pause", true);
        overview = actionMap.FindAction("Overview", true);
        focus = new InputAction[stations.Length];

        for (int i = 0; i < focus.Length; i++)
            focus[i] = actionMap.FindAction("Focus" + (i + 1), true);
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void Update()
    {
        if (replay.WasPressedThisFrame())
            Replay();
        if (pause.WasPressedThisFrame())
            SetPaused(!paused);
        if (overview.WasPressedThisFrame())
        {
            selected = -1;
            galleryCamera.transform.position = cameraStart;
            galleryCamera.orthographicSize = cameraSize;
        }

        for (int i = 0; i < focus.Length; i++)
        {
            if (!focus[i].WasPressedThisFrame())
                continue;

            selected = i;
            galleryCamera.transform.position = new Vector3(stations[i].position.x, stations[i].position.y + 2f, cameraStart.z);
            galleryCamera.orthographicSize = 3.7f;
        }

        int collisions = 0;

        foreach (ParticleCollisionCounter counter in counters)
            collisions += counter.CollisionMessages;

        string view = selected < 0 ? "All six examples" : stations[selected].name;
        status.text = $"{view}    {(paused ? "PAUSED" : "PLAYING")}    Collision callbacks: {collisions}\n1-6  Focus example    0  Overview    SPACE / R  Replay all    P  Pause / resume\nF1  Effects + audio    F2  Particles    F3  Decals    F4  Shader Graph";
    }

    private void SetPaused(bool value)
    {
        paused = value;

        foreach (ParticleSystem particles in allSystems)
        {
            if (paused && particles.isPlaying)
                particles.Pause(false);
        }

        if (!paused)
        {
            foreach (ParticleSystem particles in allSystems)
            {
                if (particles.isPaused)
                    particles.Play(false);
            }

            foreach (ParticleSystem emitter in emitters)
                emitter.Play(false);
        }

        foreach (ParticleMotion motion in motions)
            motion.SetPaused(paused);
    }

    private void Replay()
    {
        foreach (ParticleSystem particles in allSystems)
            particles.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

        foreach (ParticleMotion motion in motions)
            motion.ResetMotion();

        foreach (ParticleCollisionCounter counter in counters)
            counter.ResetCount();

        SetPaused(false);
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }
}
