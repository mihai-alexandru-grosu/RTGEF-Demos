using UnityEngine;

public class ParticleMotion : MonoBehaviour
{
    [SerializeField] private float radius = 1.6f;
    [SerializeField] private float speed = 1.5f;

    private Vector3 origin;
    private float elapsed;
    private bool paused;

    private void Awake()
    {
        origin = transform.localPosition;
    }

    private void Update()
    {
        if (paused)
            return;

        elapsed += Time.deltaTime;
        transform.localPosition = origin + new Vector3(Mathf.Sin(elapsed * speed) * radius, Mathf.Sin(elapsed * speed * 2f) * 0.4f, 0f);
    }

    public void SetPaused(bool value)
    {
        paused = value;
    }

    public void ResetMotion()
    {
        elapsed = 0f;
        transform.localPosition = origin;
    }
}
