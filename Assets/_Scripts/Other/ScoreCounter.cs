using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public int Score { get; set; }

    public void Start()
    {
        UpdateScoreText(0);
        EventManager.OnScoreUpdated += UpdateScoreText;
    }

    private void UpdateScoreText(int addedPoints)
    {
        Score += addedPoints;
        scoreText.text = Score.ToString();
    }
}
