using UnityEngine;
using UnityEngine.UI;

public class EarnDestroyBonus : MonoBehaviour
{
    [SerializeField] private int destroyGoal;
    [SerializeField] private Bonus destroyAllBonus;
    [SerializeField] private Image statusImage;

    public int Progress { get; set; }

    private void Start()
    {
        Progress = 0;
        EventManager.OnEnemyDestroyed += UpdateTakenDownEnemiesCount;

        UpdateBonusStatus();
    }

    private void EnableBonus()
    {
        destroyAllBonus.IsBonusEnabled = true;
        Progress = 0;
    }

    private void UpdateTakenDownEnemiesCount()
    {
        if (!destroyAllBonus.IsBonusEnabled)
        {
            Progress++;
            UpdateBonusStatus();
        }

        if (Progress >= destroyGoal)
            EnableBonus();
    }

    private void UpdateBonusStatus()
    {
        statusImage.fillAmount = (float)Progress / (float)destroyGoal;
    }
}
