using UnityEngine;

namespace Lecture03
{
    public class Carrot : MonoBehaviour
    {
        [SerializeField] private DemoControls demo;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<Player>())
                return;

            demo.Collect();
            gameObject.SetActive(false);
        }
    }
}
