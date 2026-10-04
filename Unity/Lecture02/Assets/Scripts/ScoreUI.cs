using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TMP_Text scoreText;

    // Runtime state.
    private GameManager gameManager;
    private int displayedScore = -1;

    private void Start()
    {
        gameManager = GameManager.Instance;

        RefreshScore();
    }

    private void Update()
    {
        if (displayedScore != gameManager.Score)
            RefreshScore();
    }

    private void RefreshScore()
    {
        if (scoreText == null)
            return;

        displayedScore = gameManager.Score;
        scoreText.text = $"SCORE  {displayedScore:00}";
    }
}
