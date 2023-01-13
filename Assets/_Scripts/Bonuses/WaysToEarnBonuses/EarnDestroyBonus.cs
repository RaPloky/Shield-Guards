public class EarnDestroyBonus : EarnBonus
{
    private void Start()
    {
        _goal = GameManager.DemolitionGoal;
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

        if (Progress >= _goal)
            EnableBonus();
    }

}
