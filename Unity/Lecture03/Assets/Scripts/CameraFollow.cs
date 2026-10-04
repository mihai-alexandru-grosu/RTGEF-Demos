using UnityEngine;

namespace Lecture03
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector2 horizontalLimits = new Vector2(8f, 52f);
        [SerializeField] private float height = 4f;

        private void LateUpdate()
        {
            transform.position = new Vector3(Mathf.Clamp(target.position.x, horizontalLimits.x, horizontalLimits.y), height, -10f);
        }
    }
}
