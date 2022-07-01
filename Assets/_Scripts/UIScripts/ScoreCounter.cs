using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    static public int currentScore;
    static public TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI scoreComponent;
    [SerializeField] DifficultyManager manager;
    private SatelliteBehavior _thatSatellite;

    private void Awake()
    {
        _thatSatellite = GetComponent<SatelliteBehavior>();
        scoreText = scoreComponent;
    }
    private void OnMouseDown()
    {
        AddScore();
        scoreText.text = currentScore.ToString();
    }
    private void AddScore()
    {
        currentScore += (int)_thatSatellite.energyIncrement * manager.satellites.Length;
    }
}
