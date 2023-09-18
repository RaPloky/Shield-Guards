public class EarnDestroyBonus : EarnBonus
{
    private void Start()
    {
        if (UpgradeManager.Instance != null)
        {
            _upgradeManager = UpgradeManager.Instance;
            _goal = _upgradeManager.CurrChargeGoalValue;
        }
        else
        {
            _goal = int.MaxValue;
        }
        Progress = 0;
        UpdateBonusStatus();
        UpdateProgressTip();
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
            UpdateProgressTip();
        }

        if (Progress >= _goal)
            EnableBonus();
    }

    public override void UpdateProgressTip()
    {
        bonusProgress.text = $"{Progress}/{_goal}\nenemies"; 
    }

}
