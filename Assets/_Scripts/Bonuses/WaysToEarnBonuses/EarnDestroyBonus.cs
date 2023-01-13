public class EarnDestroyBonus : EarnBonus
{
    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;
        _goal = _upgradeManager.CurrDemolitionGoalValue;
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
