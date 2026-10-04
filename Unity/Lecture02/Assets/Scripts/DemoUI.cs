using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoUI : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TMP_Text statusText;

    [Header("Controls")]
    [SerializeField] private Button bonusButton;

    // Runtime state.
    private GameManager gameManager;
    private bool bonusLoading;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.rKey.wasPressedThisFrame)
            Restart();
        else if (keyboard.nKey.wasPressedThisFrame)
            NextLevel();
        else if (keyboard.bKey.wasPressedThisFrame)
            LoadBonus();
    }

    public void Restart()
    {
        gameManager.RestartDemo();
    }

    public void NextLevel()
    {
        gameManager.NextLevel();
    }

    public void LoadBonus()
    {
        if (bonusLoading || SceneManager.GetSceneByName("BonusArea").isLoaded)
            return;

        bonusLoading = true;
        bonusButton.interactable = false;

        SceneManager.LoadScene("BonusArea", LoadSceneMode.Additive);
        statusText.text = "Bonus coins added. Your current level stays loaded.";
    }
}
