using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    public int currentScore;
    public int scoreMultiplier;

    void Awake()
    {
        _scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _scoreText.text = currentScore.ToString();
    }
}
