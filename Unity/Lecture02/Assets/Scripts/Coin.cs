using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private int value = 1;

    [Header("Animation")]
    [SerializeField] private float rotationSpeed = 90f;

    // Runtime state.
    private bool collected;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        collected = true; // Destroy happens at the end of the frame.

        GameManager.Instance.AddScore(value);
        Destroy(gameObject);
    }
}
