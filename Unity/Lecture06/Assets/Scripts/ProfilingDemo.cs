using UnityEngine;
using Unity.Profiling;

public class ProfilingDemo : MonoBehaviour
{
    [Header("Same workload in both modes")]
    [SerializeField] private Transform[] objects;
    [SerializeField, Range(1, 500)] private int repetitions = 150;
    [SerializeField] private bool useCachedValue = true;

    [Header("Runtime observation")]
    [SerializeField] private float workMilliseconds;

    private static readonly ProfilerMarker repeatedMarker = new ProfilerMarker("Lecture06.RepeatedCalculation");
    private static readonly ProfilerMarker cachedMarker = new ProfilerMarker("Lecture06.CachedCalculation");
    private float[] cachedValues;
    private double totalMilliseconds;
    private int sampleCount;

    public string Status => (useCachedValue ? "CACHED" : "REPEATED") + " calculation\n" +
        objects.Length + " objects / " + repetitions + " repeated calculations per object\n" +
        "Work only: " + workMilliseconds.ToString("F3") + " ms (120-frame average)\n" +
        "Intentionally amplified CPU example. Compare the named Profiler markers.";

    private void Awake()
    {
        cachedValues = new float[objects.Length];

        for (int i = 0; i < objects.Length; i++)
            cachedValues[i] = CalculateHeight(i);
    }

    private float CalculateHeight(int index)
    {
        float sum = 0f;

        for (int j = 0; j < repetitions; j++)
            sum += Mathf.PerlinNoise(index * 0.13f, j * 0.017f);

        return sum / repetitions * 2f;
    }

    private void Update()
    {
        long start = System.Diagnostics.Stopwatch.GetTimestamp();

        using ((useCachedValue ? cachedMarker : repeatedMarker).Auto())
        {
            float wave = Mathf.Sin(Time.time) * 0.2f;

            for (int i = 0; i < objects.Length; i++)
            {
                float height = useCachedValue ? cachedValues[i] : CalculateHeight(i);
                var position = objects[i].localPosition;
                position.y = height + wave;
                objects[i].localPosition = position;
            }
        }

        totalMilliseconds += (System.Diagnostics.Stopwatch.GetTimestamp() - start) * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
        sampleCount++;

        if (sampleCount >= 120)
        {
            workMilliseconds = (float)(totalMilliseconds / sampleCount);
            totalMilliseconds = 0;
            sampleCount = 0;
        }
    }

    public void ToggleMode()
    {
        useCachedValue = !useCachedValue;
        totalMilliseconds = 0;
        sampleCount = 0;
        workMilliseconds = 0f;
    }

    public void ResetDemo()
    {
        useCachedValue = true;
        totalMilliseconds = 0;
        sampleCount = 0;
        workMilliseconds = 0f;
    }
}
