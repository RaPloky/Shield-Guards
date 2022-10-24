using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;

    private int _activeGuardsCount;

    public int ActiveGuardsCount
    {
        get => _activeGuardsCount;
        set => _activeGuardsCount = value;
    }

    private void Start()
    {
        ActiveGuardsCount = GameObject.FindGameObjectsWithTag("Guard").Length;
        EventManager.OnGuardDischarged += ReduceActiveGuardsCount;
    }

    private void OnDisable()
    {
        EventManager.OnGuardDischarged -= ReduceActiveGuardsCount;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
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

    private void LoseGame()
    {
        // lose animation
        losePanel.SetActive(true);
    }

    private void ReduceActiveGuardsCount()
    {
        ActiveGuardsCount--;

        if (Mathf.Approximately(ActiveGuardsCount, 0))
            LoseGame();
    }
}
