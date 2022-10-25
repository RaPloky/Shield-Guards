public class EarnDestroyBonus : EarnBonus
{
    private void Start()
    {
        Progress = 0;

        UpdateBonusStatus();
    }

    private void OnEnable()
    {
        EventManager.OnEnemyDestroyed += UpdateTakenDownEnemiesCount;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyDestroyed -= UpdateTakenDownEnemiesCount;
    }

    private void UpdateTakenDownEnemiesCount()
    {
        if (!bonus.IsBonusEnabled)
        {
            Progress++;
            UpdateBonusStatus();
        }

        if (Progress >= goal)
            EnableBonus();
    }

}
