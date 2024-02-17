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
    [SerializeField] private GameObject goToMenuPanel;
    [SerializeField] private GameObject navigationCanvas;
    [SerializeField] private Score score;
    [SerializeField] private TextMeshProUGUI endScore;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI activeGuardsCountUI;
    [SerializeField] private DifficultyUpdate difficultyManager;
    [SerializeField] private bool isMenu;
    [SerializeField] private bool isTutorial;
    [SerializeField, Range(2f, 10f)] private float loseDelay;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private Button pauseButton;
    [SerializeField] private EnemyInvokeOnGuardLose enemyInvoke;
    [SerializeField] private GameObject heartbeatPanel;
    [SerializeField] private CanvasGroup doubleRewardButton;

    public static int DefaultChargingGoal => 10000;
    public static int DefaultDemolitionGoal => 20;
    public static int DefaultProtectionGoal => 60;

    private int _activeGuardsCount = 0;
    private int _currencyToAdd = 0;
    public static bool IsGamePaused;

    public string TutorialFinished_Pref => "TutorialFinished";
    public string TutorialNotFinishedStance => "notFinished";
    public string TutorialFinishedStance => "finished";

    public string TutorialCompletedEver_Pref => "TutorialCompletedEver";
    public string TutorialCompletedEverStance => "completedEver";
    public string TutorialNotCompletedEverStance => "notCompletedEver";

    public int ActiveGuardsCount
    {
        get => _activeGuardsCount;
        set => _activeGuardsCount = value;
    }

    private void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        Instance = this;
    }

    private void Start()
    {
        if (!isMenu)
        {
            if (!isTutorial)
                ActiveGuardsCount = difficultyManager.ActiveGuards.Count;

            UpdateActiveGuardsCountUI();
        }
        else if (isMenu)
            LoadBannerAd();

        if (!isMenu && !isTutorial)
            HideBannerAd();

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
        EventManager.OnRewardAdWatched += DoubleCurrency;
        IsGamePaused = false;
    }

    private void OnDisable()
    {
        EventManager.OnGuardDischarged -= ReduceActiveGuardsCount;
        EventManager.OnGuardDischarged -= UpdateActiveGuardsCountUI;
        EventManager.OnRewardAdWatched -= DoubleCurrency;
        IsGamePaused = false;

        AddCurrency();
    }

    private void LoadBannerAd() => LoadBanner.Instance.LoadTheBanner();
    private void HideBannerAd() => LoadBanner.Instance.HideBanner();
    private void DoubleCurrency()
    {
        _currencyToAdd *= 2;
        doubleRewardButton.interactable = false;
        doubleRewardButton.alpha = 0;
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

        if (goToMenuPanel != null)
            goToMenuPanel.SetActive(true);
    }

    public void ExitToMenuFromTutotial()
    {
        ExitToMenu();
        TutorialManager.Instance.CompleteTutorial();
    }

    public void StartGame()
    {
        UnpauseGame();
        StartCoroutine(LoadDelayedScene("Game"));
    }

    public void StartTutorial()
    {
        ResetTutorialStance();
        UnpauseGame();
        StartCoroutine(LoadDelayedScene("TutorialScene"));
    }

    private void ResetTutorialStance()
    {
        PlayerPrefs.SetString(TutorialFinished_Pref, TutorialNotFinishedStance);
    }

    private IEnumerator LoadDelayedScene(string sceneName)
    {
        LoadBanner.Instance.HideBanner();

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
        navigationCanvas.SetActive(false);
        pauseButton.interactable = false;
        shieldAnimator.SetTrigger("GameLosed");
        BG_Music.Instance.FadeOut();

        yield return new WaitForSeconds(loseDelay);
        losePanel.SetActive(true);

        if (score.ScoreAmount > PlayerPrefs.GetInt(Score.ScorePref))
            score.UpdateBestScore();

        AssignEndScore();

        StartCoroutine(LoadInterstitialAd(3f));
        Invoke(nameof(LoadBannerAd), 4f);

        _currencyToAdd = score.ScoreAmount;
    }

    private IEnumerator LoadInterstitialAd(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        LoadInterstitial.Instance.LoadAd();
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
        // Player possibly may abuse tutorial for easy
        // funds, here's prevention:
        if (isTutorial)
            return;

        int playerCurrencyCount = PlayerPrefs.GetInt(UpgradeManager.EnergyPref, 0);
        PlayerPrefs.SetInt(UpgradeManager.EnergyPref, playerCurrencyCount + _currencyToAdd);
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
