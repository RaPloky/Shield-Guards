using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private TextMeshProUGUI endScore;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI activeGuardsCountUI;
    [SerializeField] private DifficultyUpdate difficultyManager;
    [SerializeField] private bool isMenu;
    [SerializeField, Range(2f, 10f)] private float loseDelay;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private Button pauseButton;
    [SerializeField] private EnemyInvokeOnGuardLose enemyInvoke;
    [SerializeField] private GameObject heartbeatPanel;

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
        Application.targetFrameRate = 60;
        Instance = this;
    }

    private void Start()
    {
        if (!isMenu)
        {
            ActiveGuardsCount = difficultyManager.ActiveGuards.Count;
            UpdateActiveGuardsCountUI();
        }

        if (bestScore != null)
        {
            int score = PlayerPrefs.GetInt(Score.ScorePref);
            bestScore.text = score > 0 ? "Best score: " + score : string.Empty;
        }

        PlayBG_Music();
    }

    private void OnEnable()
    {
        EventManager.OnGuardDischarged += ReduceActiveGuardsCount;
        EventManager.OnGuardDischarged += UpdateActiveGuardsCountUI;
        IsGamePaused = false;
    }

    private void OnDisable()
    {
        EventManager.OnGuardDischarged -= ReduceActiveGuardsCount;
        EventManager.OnGuardDischarged -= UpdateActiveGuardsCountUI;
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
        StartCoroutine(LoadDelayedScene(SceneManager.GetActiveScene().name));
    }

    public void ExitToMenu()
    {
        UnpauseGame();
        StartCoroutine(LoadDelayedScene("Menu"));
    }

    public void StartGame()
    {
        UnpauseGame();
        StartCoroutine(LoadDelayedScene("Game"));
    }

    private IEnumerator LoadDelayedScene(string sceneName)
    {
        BG_Music.Instance.FadeOut();
        yield return new WaitForSeconds(BG_Music.Instance.FadeDuration);
        SceneManager.LoadScene(sceneName);
    }

    private void LoseGame()
    {
        StartCoroutine(Lose());
    }

    private IEnumerator Lose()
    {
        pauseButton.interactable = false;
        shieldAnimator.SetTrigger("GameLosed");
        BG_Music.Instance.FadeOut();

        yield return new WaitForSeconds(loseDelay);
        losePanel.SetActive(true);

        if (score.ScoreAmount > PlayerPrefs.GetInt(Score.ScorePref))
            score.UpdateBestScore();

        AssignEndScore();
        AddCurrency();
    }

    private void ReduceActiveGuardsCount()
    {
        ActiveGuardsCount--;

        if (IsItTimeToActivateCarriers())
            ActivateCarriers();

        if (IsItTimeToActivateMicronovas())
        {
            ActivateMicronovas();
            ActivateHeartbeat(true);
        }

        if (IsGameLosed())
        {
            LoseGame();
            ActivateHeartbeat(false);
        }
    }

    private bool IsItTimeToActivateCarriers() => Mathf.Approximately(ActiveGuardsCount, 2);
    private void ActivateCarriers() => enemyInvoke.ActivateCarrierSpawners();

    private bool IsItTimeToActivateMicronovas() => Mathf.Approximately(ActiveGuardsCount, 1);
    private void ActivateMicronovas() => enemyInvoke.ActivateMicronovaSpanwers();

    private bool IsGameLosed() => Mathf.Approximately(ActiveGuardsCount, 0);

    private void AddCurrency()
    {
        int storedEnergy = PlayerPrefs.GetInt(UpgradeManager.EnergyPref, 0);
        PlayerPrefs.SetInt(UpgradeManager.EnergyPref, score.ScoreAmount + storedEnergy);
    }

    private void ActivateHeartbeat(bool isEnabled)
    {
        if (isEnabled)
            heartbeatPanel.SetActive(true);
        else
            heartbeatPanel.SetActive(false);
    }

    private void AssignEndScore() => endScore.text = $"you scored: {score.ScoreAmount}";
    private void UpdateActiveGuardsCountUI() => 
        activeGuardsCountUI.text = Mathf.Approximately(ActiveGuardsCount, 0) ? string.Empty : $"X{ActiveGuardsCount}";

    private void PlayBG_Music() => BG_Music.Instance.StartPlay();
}
