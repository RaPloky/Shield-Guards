using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI _score;

    public string ScorePref => "BestScore";

    public int ScoreAmount => int.Parse(_score.text);

    private void Start()
    {
        _score = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateBestScore()
    {
        PlayerPrefs.SetInt(ScorePref, int.Parse(_score.text));
    }

}
