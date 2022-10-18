public class EarnDestroyBonus : EarnBonus
{
    private void Start()
    {
        Progress = 0;
        EventManager.OnEnemyDestroyed += UpdateTakenDownEnemiesCount;

        UpdateBonusStatus();
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
