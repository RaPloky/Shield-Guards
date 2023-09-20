using System.Collections;
using UnityEngine;

public class EarnProtectBonus : EarnBonus
{
    [SerializeField, Range(0.5f, 1f)] private float surviveTimeUpdateAmount;

    private void Start()
    {
        if (UpgradeManager.Instance != null)
        {
            _upgradeManager = UpgradeManager.Instance;
            _goal = _upgradeManager.CurrProtectionGoalValue;
        }
        else
        {
            _goal = int.MaxValue;
        }
        Progress = 0;

        StartCoroutine(UpdateSecondsSurvived());
        UpdateBonusStatus(); 
        UpdateProgressTip();
    }

    private IEnumerator UpdateSecondsSurvived()
    {
        while (true)
        {
            if (!bonus.isActiveAndEnabled)
                yield break;

            yield return new WaitForSeconds(surviveTimeUpdateAmount);

            if (!bonus.IsBonusEnabled)
            {
                Progress += surviveTimeUpdateAmount;
                UpdateBonusStatus();
                UpdateProgressTip();
            }

            if (Progress >= _goal)
                EnableBonus();
        }
    }

    public override void UpdateProgressTip()
    {
        bonusProgress.text = $"{Progress:0.0}/{_goal:0.0}S";
    }
}
