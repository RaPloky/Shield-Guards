using UnityEngine;
using TMPro;

public class GetHighScoreInMenu : MonoBehaviour
{
    private TextMeshProUGUI _highScore;

    private void Start()
    {
        _highScore = gameObject.GetComponent<TextMeshProUGUI>();
        _highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
}
