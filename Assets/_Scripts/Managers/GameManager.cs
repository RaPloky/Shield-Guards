using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Score score;
    [SerializeField] private TextMeshProUGUI bestScore;

    private int _activeGuardsCount;

    public static bool IsGamePaused;

    public int ActiveGuardsCount
    {
        get => _activeGuardsCount;
        set => _activeGuardsCount = value;
    }

    private void Start()
    {
        ActiveGuardsCount = GameObject.FindGameObjectsWithTag("Guard").Length;

        if (bestScore != null)
            bestScore.text = "Best score\n" + PlayerPrefs.GetInt(Score.ScorePref);
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
        // loading animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        // loading animation
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        // loading animation
        SceneManager.LoadScene("Game");
    }

    private void LoseGame()
    {
        // lose animation
        losePanel.SetActive(true);

        if (score.ScoreAmount > PlayerPrefs.GetInt(Score.ScorePref))
            score.UpdateBestScore();

        PlayerPrefs.SetInt(UpgradeManager.CurrencyPref, score.ScoreAmount);
;    }

    private void ReduceActiveGuardsCount()
    {
        ActiveGuardsCount--;

        if (Mathf.Approximately(ActiveGuardsCount, 0))
            LoseGame();
    }
}
