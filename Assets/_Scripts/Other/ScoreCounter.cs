using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public int Score { get; set; }

    private void Start()
    {
        UpdateScoreText(0);
    }

    private void OnEnable()
    {
        EventManager.OnScoreUpdated += UpdateScoreText;
    }

    private void OnDisable()
    {
        EventManager.OnScoreUpdated -= UpdateScoreText;
    }

    private void UpdateScoreText(int addedPoints)
    {
        Score += addedPoints;
        scoreText.text = Score.ToString();
    }
}
