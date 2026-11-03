using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LectureSixUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text preferenceText;
    [SerializeField] private NavigationDemo navigation;
    [SerializeField] private LightingDemo lighting;
    [SerializeField] private ProfilingDemo profiling;

    [SerializeField] private TMP_Dropdown colorDropdown;

    private const string PreferenceKey = "RTGEF.Lecture06.TextColor";
    private int colorIndex;
    private TMP_Text[] sceneTexts;

    private static readonly Color[] TextColors =
    {
        Color.white,
        new Color(1f, 0.82f, 0.35f),
        new Color(0.4f, 0.9f, 1f),
        new Color(0.55f, 1f, 0.65f),
        new Color(1f, 0.65f, 0.85f)
    };
    private float nextRefresh;

    private void Start()
    {
        sceneTexts = Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        colorIndex = Mathf.Clamp(PlayerPrefs.GetInt(PreferenceKey, 0), 0, TextColors.Length - 1);
        ApplyPreference();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.f1Key.wasPressedThisFrame)
                OpenScene(0);
            if (keyboard.f2Key.wasPressedThisFrame)
                OpenScene(1);
            if (keyboard.rKey.wasPressedThisFrame)
                ResetDemo();
        }

        if (Time.unscaledTime < nextRefresh)
            return;

        nextRefresh = Time.unscaledTime + 0.2f;
        statusText.text = navigation ? navigation.Status : lighting ? lighting.Status : profiling.Status;
    }

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetTextColor(int index)
    {
        colorIndex = Mathf.Clamp(index, 0, TextColors.Length - 1);
        PlayerPrefs.SetInt(PreferenceKey, colorIndex);
        PlayerPrefs.Save();
        ApplyPreference();
    }

    public void ClearPreference()
    {
        PlayerPrefs.DeleteKey(PreferenceKey);
        PlayerPrefs.Save();
        colorIndex = 0;
        ApplyPreference();
    }

    private void ApplyPreference()
    {
        colorDropdown.SetValueWithoutNotify(colorIndex);
        preferenceText.text = "PlayerPrefs: " + colorDropdown.options[colorIndex].text + " text (saved between sessions)";

        foreach (var text in sceneTexts)
            text.color = TextColors[colorIndex];
    }

    public void ResetDemo()
    {
        if (navigation)
            navigation.ResetDemo();
        if (lighting)
            lighting.ResetDemo();
        if (profiling)
            profiling.ResetDemo();
    }
}
