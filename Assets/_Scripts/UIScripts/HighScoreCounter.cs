using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI score;

    private TextMeshProUGUI _highScore;
    private int currentScore;

    private void Start()
    {
        _highScore = gameObject.GetComponent<TextMeshProUGUI>();
        _highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    private void Update()
    {
        currentScore = int.Parse(score.text);
        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            _highScore.text = currentScore.ToString();
        }
    }
}