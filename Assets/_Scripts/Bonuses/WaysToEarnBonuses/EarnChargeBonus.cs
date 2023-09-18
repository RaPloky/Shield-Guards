using UnityEngine;

public class EarnChargeBonus : EarnBonus
{
    [SerializeField] private int updateProgressAmount;

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
        EventManager.OnNonBonusEnergyAdded += UpdateAddedEnergyAmount;
    }

    private void OnDisable()
    {
        EventManager.OnNonBonusEnergyAdded -= UpdateAddedEnergyAmount;
    }

    private void UpdateAddedEnergyAmount()
    {
        if (!bonus.IsBonusEnabled)
        {
            Progress += updateProgressAmount;
            UpdateBonusStatus();
            UpdateProgressTip();
        }

        if (Progress >= _goal)
            EnableBonus();
    }

    public override void UpdateProgressTip()
    {
        bonusProgress.text = $"{Progress / 1000:0}k/{_goal / 1000:0}k\nenergy";
    }
}
