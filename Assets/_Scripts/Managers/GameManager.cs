using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BonusGoal
{
    None,
    Charging,
    Demolition,
    Protection
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private Score score;
    [SerializeField] private TextMeshProUGUI bestScore;

    public static int DefaultChargingGoal => 10000;
    public static int DefaultDemolitionGoal => 20;
    public static int DefaultProtectionGoal => 60;

    private int _activeGuardsCount;

    public static bool IsGamePaused;

    public int ActiveGuardsCount
    {
        get => _activeGuardsCount;
        set => _activeGuardsCount = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ActiveGuardsCount = GameObject.FindGameObjectsWithTag("Guard").Length;

        if (bestScore != null)
        {
            int score = PlayerPrefs.GetInt(Score.ScorePref);
            bestScore.text = score > 0 ? "Best score: " + score : string.Empty;
        }
    }

    private void OnEnable()
    {
        EventManager.OnGuardDischarged += ReduceActiveGuardsCount;
        IsGamePaused = false;
    }

    private void OnDisable()
    {
        EventManager.OnGuardDischarged -= ReduceActiveGuardsCount;
        IsGamePaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void RestartGame()
    {
        UnpauseGame();
        // loading animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        UnpauseGame();
        // loading animation
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        // loading animation
        UnpauseGame();
        SceneManager.LoadScene("Game");
    }

    private void LoseGame()
    {
        // lose animation
        losePanel.SetActive(true);
        PauseGame();

        if (score.ScoreAmount > PlayerPrefs.GetInt(Score.ScorePref))
            score.UpdateBestScore();

        UpdateEnergyCount();
    }

    private void ReduceActiveGuardsCount()
    {
        ActiveGuardsCount--;

        if (Mathf.Approximately(ActiveGuardsCount, 0))
            LoseGame();
    }

    private void UpdateEnergyCount()
    {
        int storedEnergy = PlayerPrefs.GetInt(UpgradeManager.EnergyPref, 0);
        PlayerPrefs.SetInt(UpgradeManager.EnergyPref, score.ScoreAmount + storedEnergy);
    }
}
