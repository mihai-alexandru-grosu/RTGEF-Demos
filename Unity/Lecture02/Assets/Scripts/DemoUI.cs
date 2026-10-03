using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoUI : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text statusText;

    [Header("Controls")]
    [SerializeField] private Button bonusButton;

    // Runtime state.
    private GameManager gameManager;
    private int displayedScore = -1;
    private bool bonusLoading;

    private void Start()
    {
        gameManager = GameManager.Instance;

        RefreshScore();
    }

    private void Update()
    {
        if (displayedScore != gameManager.Score)
            RefreshScore();

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

    private void RefreshScore()
    {
        if (scoreText == null)
            return;

        displayedScore = gameManager.Score;
        scoreText.text = $"SCORE  {displayedScore:00}";
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
