using System.Collections;
using UnityEngine;

public class EarnProtectBonus : EarnBonus
{
    [SerializeField, Range(0.5f, 1f)] private float surviveTimeUpdateAmount;

    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;
        _goal = _upgradeManager.CurrProtectionGoalValue;
        Progress = 0;

        StartCoroutine(UpdateSecondsSurvived());
        UpdateBonusStatus();
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
            }

            if (Progress >= _goal)
                EnableBonus();
        }
    }
}
