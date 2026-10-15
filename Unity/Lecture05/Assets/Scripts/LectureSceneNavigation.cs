using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LectureSceneNavigation : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button[] sceneButtons;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateNavigation()
    {
        var navigation = Instantiate(Resources.Load<GameObject>("LectureSceneNavigation"));
        DontDestroyOnLoad(navigation);
    }

    private InputAction[] sceneActions;
    private readonly string[] sceneNames =
    {
        "Demo", "ParticleExamples", "DecalExamples", "ShaderExamples"
    };

    private void Awake()
    {
        sceneActions = new InputAction[sceneNames.Length];

        for (int i = 0; i < sceneActions.Length; i++)
        {
            sceneActions[i] = new InputAction("Scene" + (i + 1), InputActionType.Button, "<Keyboard>/f" + (i + 1));
            int sceneIndex = i;
            sceneButtons[i].onClick.AddListener(() => SwitchScene(sceneIndex));
        }
    }

    private void OnEnable()
    {
        foreach (InputAction action in sceneActions)
            action.Enable();

        SceneManager.sceneLoaded += OnSceneLoaded;
        RefreshButtons(SceneManager.GetActiveScene());
    }

    private void Update()
    {
        for (int i = 0; i < sceneActions.Length; i++)
        {
            if (!sceneActions[i].WasPressedThisFrame())
                continue;

            SwitchScene(i);
            return;
        }
    }

    private void SwitchScene(int index)
    {
        if (SceneManager.GetActiveScene().name == sceneNames[index])
            return;

        SceneManager.LoadScene(sceneNames[index]);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshButtons(scene);
    }

    private void RefreshButtons(Scene scene)
    {
        for (int i = 0; i < sceneButtons.Length; i++)
            sceneButtons[i].interactable = scene.name != sceneNames[i];

        if (scene.name != "Demo")
            return;

        // Leave room below the title for the shared navigation bar.
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (UnityEngine.UI.Text text in root.GetComponentsInChildren<UnityEngine.UI.Text>())
            {
                if (text.name == "Status")
                    text.rectTransform.anchoredPosition = new Vector2(30f, -130f);
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        foreach (InputAction action in sceneActions)
            action.Disable();
    }

    private void OnDestroy()
    {
        foreach (InputAction action in sceneActions)
            action.Dispose();
    }
}
