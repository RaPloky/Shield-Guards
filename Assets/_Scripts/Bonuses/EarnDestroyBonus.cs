using UnityEngine;

public class EarnDestroyBonus : MonoBehaviour
{
    [SerializeField] private int destroyGoal;
    [SerializeField] private Bonus destroyAllBonus;

    public int Progress { get; set; }
    public int Goal => destroyGoal;

    private void Start()
    {
        Progress = 0;
        EventManager.OnEnemyDestroyed += UpdateTakenDownEnemiesCount;
    }

    private void EnableBonus()
    {
        destroyAllBonus.IsBonusEnabled = true;
        destroyAllBonus.FillStatusIndicator();
        Progress = 0;
    }

    private void UpdateTakenDownEnemiesCount()
    {
        if (!destroyAllBonus.IsBonusEnabled)
            Progress++;

        if (Progress >= destroyGoal)
            EnableBonus();
    }
}
