using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Runtime state")]
    [SerializeField] private int score;

    public static GameManager Instance
    {
        get;
        private set;
    }

    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void RestartDemo()
    {
        score = 0;

        SceneManager.LoadScene("Demo", LoadSceneMode.Single);
    }

    public void NextLevel()
    {
        string next = SceneManager.GetActiveScene().name == "Demo" ? "Level2" : "Demo";

        SceneManager.LoadScene(next, LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}

