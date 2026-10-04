using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Lecture03
{
    public class DemoControls : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionAsset actions;

        [Header("Display")]
        [SerializeField] private TMPro.TMP_Text status;
        [SerializeField] private string sceneTitle;

        [Header("Runtime observation")]
        [SerializeField] private int carrots;
        [SerializeField] private string lastPhase = "Hold E to observe Started / Performed / Canceled";

        // Runtime state.
        private InputActionMap demo;

        private void OnEnable()
        {
            demo = actions.FindActionMap("Demo", true);
            demo.actionTriggered += OnAction;
            demo.Enable();
        }

        private void OnDisable()
        {
            demo.actionTriggered -= OnAction;
            demo.Disable();
        }

        private void OnAction(InputAction.CallbackContext context)
        {
            string actionName = context.action.name;

            if (actionName == "Charge" || actionName == "Modified")
            {
                lastPhase = actionName + ": " + context.phase;
                Debug.Log(lastPhase, this);
            }

            if (!context.performed)
                return;

            if (actionName == "Restart")
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (actionName == "Platformer")
                SceneManager.LoadScene("Platformer");
            if (actionName == "TileWorkshop")
                SceneManager.LoadScene("TileWorkshop");
            if (actionName == "Quit")
                Application.Quit();
        }

        private void Update()
        {
            status.text = sceneTitle + "   |   Carrots: " + carrots + "\nA/D or arrows / stick: move    Space / south button: jump    R: reset\n1: Level   3: Tile workshop   Esc: quit\n" + lastPhase;
        }

        public void Collect()
        {
            carrots++;
        }
    }
}
