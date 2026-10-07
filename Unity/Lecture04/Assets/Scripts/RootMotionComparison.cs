using TMPro;
using UnityEngine;

public class RootMotionComparison : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator[] animators;
    [SerializeField] private TMP_Text statusText;

    [Header("Loop duration")]
    [SerializeField] private float comparisonDuration = 4f;

    // Runtime state.
    private Vector3[] homes;
    private float elapsed;

    private void Start()
    {
        homes = new Vector3[animators.Length];

        for (int i = 0; i < animators.Length; i++)
            homes[i] = animators[i].transform.position;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        statusText.text = $"Time: {elapsed:0.0} s\nApply Root Motion OFF: {Distance(0):0.00} m\nApply Root Motion ON: {Distance(1):0.00} m\nOnAnimatorMove x 0.5: {Distance(2):0.00} m\nRepeats every {comparisonDuration:0} seconds";

        if (elapsed < comparisonDuration)
            return;

        elapsed = 0f;

        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].transform.position = homes[i];
            animators[i].transform.rotation = Quaternion.identity;
            animators[i].Play("Walk", 0, 0f);
        }
    }

    private float Distance(int index)
    {
        return Vector3.Distance(animators[index].transform.position, homes[index]);
    }
}
