using UnityEngine;

public class RootMotionDriver : MonoBehaviour
{
    [Header("Root motion")]
    [SerializeField] private Animator animator;
    [SerializeField, Range(0f, 2f)] private float distanceScale = 0.5f;

    private void OnAnimatorMove()
    {
        // Unity supplies this frame's displacement. No movement input is involved.
        transform.position += animator.deltaPosition * distanceScale;
        transform.rotation *= animator.deltaRotation;
    }
}
