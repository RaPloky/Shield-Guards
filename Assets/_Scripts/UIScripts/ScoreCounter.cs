using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    static public int currentScore;
    static public TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI scoreComponent;
    [SerializeField] TextMeshProUGUI highScore;
    [SerializeField] DifficultyManager manager;
    private SatelliteBehavior _thatSatellite;

    private void Awake()
    {
        _thatSatellite = GetComponent<SatelliteBehavior>();
        scoreText = scoreComponent;
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    private void OnMouseDown()
    {
        AddScore();
        scoreText.text = currentScore.ToString();

        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
            UpdateHighScore();
    }
    private void AddScore()
    {
        currentScore += (int)_thatSatellite.energyIncrement * manager.satellites.Length;
    }
    private void UpdateHighScore()
    {
        PlayerPrefs.SetInt("HighScore", currentScore);
        highScore.text = currentScore.ToString();
    }
}
