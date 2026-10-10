using UnityEngine;

namespace Lecture03
{
    public class EffectorVisuals : MonoBehaviour
    {
        [Header("Conveyor")]
        [SerializeField] private SurfaceEffector2D conveyor;

        [SerializeField] private float conveyorSpeed;
        [SerializeField] private Transform[] treads;

        [Header("Wind")]
        [SerializeField] private AreaEffector2D wind;
        [SerializeField] private Transform[] leaves;

        private void Update()
        {
            if (conveyor && conveyor.isActiveAndEnabled)
            {
                foreach (var tread in treads)   
                {
                    var position = tread.localPosition;
                    position.x = -2.8f + Mathf.Repeat(position.x + 2.8f + conveyorSpeed * Time.deltaTime, 5.6f);
                    tread.localPosition = position;
                }
            }

            if (!wind || !wind.isActiveAndEnabled)
                return;

            for (int i = 0; i < leaves.Length; i++)
            {
                float phase = Time.time * 0.45f + i / (float)leaves.Length;
                float height = Mathf.Repeat(phase, 1f);
                float x = Mathf.Sin(phase * Mathf.PI * 2f + i * 2.4f) * 1.45f;
                leaves[i].localPosition = new Vector3(x, 1.1f + height * 5.1f, 0f);
                leaves[i].localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(phase * 7f + i) * 35f);
            }
        }
    }
}
