using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Follow target")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 13f, -12f);

    private void LateUpdate()
    {
        if (target == null)
            return;
        
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }
}
