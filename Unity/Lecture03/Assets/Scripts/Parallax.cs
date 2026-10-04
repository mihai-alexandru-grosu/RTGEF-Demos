using UnityEngine;

namespace Lecture03
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [Range(0f, 1f)]
        [SerializeField] private float parallaxEffect = 0.7f;

        // Runtime state.
        private Vector3 initialPosition;
        private Vector3 initialCameraPosition;

        private void Start()
        {
            initialPosition = transform.position;
            initialCameraPosition = cameraTransform.position;
        }

        private void LateUpdate()
        {
            transform.position = initialPosition + new Vector3((cameraTransform.position.x - initialCameraPosition.x) * parallaxEffect, 0f, 0f);
        }
    }
}
