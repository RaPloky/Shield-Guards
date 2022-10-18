using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectAllGuards : Bonus
{
    [Header("Resist bonus")]
    [SerializeField] private List<Guard> guards;
    [SerializeField, Range(4f, 8f)] private float shieldDuration;

    [Header("Time warp bonus")]
    [SerializeField] private float timeWarpDuration;
    [SerializeField, Range(0.1f, 0.5f)] private float timeWarpValue;

    public void ActivateShieldBonus()
    {
        if (!isBonusEnabled)
            return;

        StartCoroutine(ProtectGuards());

        isBonusEnabled = false;
        UnfillStatusIndicator();
    }

    public void ActivateTimeWarpBonus()
    {
        if (!isBonusEnabled)
            return;

        StartCoroutine(ActivateTimeWarp());

        isBonusEnabled = false;
        UnfillStatusIndicator();
    }

    private IEnumerator ProtectGuards()
    {
        foreach (Guard guard in guards)
            guard.IsProtectBonusActivated = true;

        yield return new WaitForSeconds(shieldDuration);

        foreach (Guard guard in guards)
            guard.IsProtectBonusActivated = false;
    }

    private IEnumerator ActivateTimeWarp()
    {
        Time.timeScale = timeWarpValue;
        Time.fixedDeltaTime = Time.timeScale * .02f;

        yield return new WaitForSecondsRealtime(timeWarpDuration);

        Time.timeScale = 1f;
    }
}
