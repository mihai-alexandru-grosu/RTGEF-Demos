using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private float distance = 2f;
    [SerializeField] private float speed = 1f;

    // Runtime state.
    private Rigidbody rb;
    private Vector3 startPosition;
    private float travelled;
    private float sign = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = rb.position;
    }

    private void FixedUpdate()
    {
        travelled += sign * speed * Time.fixedDeltaTime;

        if (travelled >= distance)
        {
            travelled = distance;
            sign = -1f;
        }

        if (travelled <= 0f)
        {
            travelled = 0f;
            sign = 1f;
        }

        rb.MovePosition(startPosition + direction.normalized * travelled);
    }
}
