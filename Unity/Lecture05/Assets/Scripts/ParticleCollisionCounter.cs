using UnityEngine;

public class ParticleCollisionCounter : MonoBehaviour
{
    [Header("Runtime observation")]
    [SerializeField] private int collisionMessages;

    public int CollisionMessages => collisionMessages;

    private void OnParticleCollision(GameObject other)
    {
        collisionMessages++;
    }

    public void ResetCount()
    {
        collisionMessages = 0;
    }
}
