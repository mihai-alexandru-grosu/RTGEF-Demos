using UnityEngine;
using UnityEngine.Rendering;

public class LightingDemo : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private Light pointLight;
    [SerializeField] private Light spotLight;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Transform probeObject;

    private bool moving = true;
    private float elapsed;

    public string Status => "Realtime point: " + pointLight.enabled + "    Spot: " + spotLight.enabled +
        "\nMixed sun: " + sun.enabled + "    Global volume: " + globalVolume.enabled +
        "\nBaked area light stays in the lightmaps. Moving sphere samples probes.";

    private void Update()
    {
        if (moving)
            elapsed += Time.deltaTime;

        probeObject.localPosition = new Vector3(Mathf.Sin(elapsed * 0.6f) * 7f, 1.2f, 1f);
    }

    public void TogglePoint()
    {
        pointLight.enabled = !pointLight.enabled;
    }

    public void ToggleSpot()
    {
        spotLight.enabled = !spotLight.enabled;
    }

    public void ToggleSun()
    {
        sun.enabled = !sun.enabled;
    }

    public void ToggleVolume()
    {
        globalVolume.enabled = !globalVolume.enabled;
    }

    public void ToggleMotion()
    {
        moving = !moving;
    }

    public void ResetDemo()
    {
        sun.enabled = true;
        pointLight.enabled = true;
        spotLight.enabled = true;
        globalVolume.enabled = true;
        moving = true;
        elapsed = 0f;
    }
}
