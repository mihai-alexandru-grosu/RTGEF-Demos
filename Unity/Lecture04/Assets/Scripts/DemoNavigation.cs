using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DemoNavigation : MonoBehaviour
{
    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.f1Key.wasPressedThisFrame)
            OpenScene(0);
        else if (keyboard.f2Key.wasPressedThisFrame)
            OpenScene(1);
        else if (keyboard.f3Key.wasPressedThisFrame)
            OpenScene(2);
        else if (keyboard.f4Key.wasPressedThisFrame)
            OpenScene(3);
        else if (keyboard.rKey.wasPressedThisFrame)
            OpenScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
