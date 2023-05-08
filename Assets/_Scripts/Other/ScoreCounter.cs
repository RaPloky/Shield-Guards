using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI justAddedScoreText;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator justAddedScoreAnimator;

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

        justAddedScoreText.text = $"+{addedPoints}";
        justAddedScoreAnimator.SetTrigger("UpdatedScore");
        scoreText.text = $"${Score}";
        animator.SetTrigger("Update");
    }
}
