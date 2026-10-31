using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LectureSixUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text statusText;
    [SerializeField] private UnityEngine.UI.Text preferenceText;
    [SerializeField] private NavigationDemo navigation;
    [SerializeField] private LightingDemo lighting;
    [SerializeField] private ProfilingDemo profiling;

    private const string PreferenceKey = "RTGEF.Lecture06.LargeText";
    private bool largeText;
    private float nextRefresh;

    private void Start()
    {
        largeText = PlayerPrefs.GetInt(PreferenceKey, 0) == 1;
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

    public void TogglePreference()
    {
        largeText = !largeText;
        PlayerPrefs.SetInt(PreferenceKey, largeText ? 1 : 0);
        PlayerPrefs.Save();
        ApplyPreference();
    }

    public void ClearPreference()
    {
        PlayerPrefs.DeleteKey(PreferenceKey);
        PlayerPrefs.Save();
        largeText = false;
        ApplyPreference();
    }

    private void ApplyPreference()
    {
        statusText.fontSize = largeText ? 23 : 19;
        preferenceText.text = "PlayerPrefs: " + (largeText ? "large" : "normal") + " status text (saved between sessions)";
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
