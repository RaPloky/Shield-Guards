using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    [SerializeField] private DifficultyUpdate difficultyManager;
    [SerializeField] private bool isMenu;
    [SerializeField, Range(2f, 10f)] private float loseDelay;

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
        if (!isMenu)
            ActiveGuardsCount = difficultyManager.ActiveGuards.Count;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        UnpauseGame();
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        UnpauseGame();
        SceneManager.LoadScene("Game");
    }

    private void LoseGame()
    {
        StartCoroutine(Lose());
    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(loseDelay);
        losePanel.SetActive(true);

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
