using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectAllGuards : Bonus
{
    [Header("Resist bonus")]
    [SerializeField] private List<Guard> guards;
    private float shieldDuration;

    private void Start()
    {
        shieldDuration = UpgradeManager.instance.CurrentProtectionValue;
    }

    public void ActivateShieldBonus()
    {
        if (!isBonusEnabled)
            return;

        StartCoroutine(ProtectGuards());

        isBonusEnabled = false;
        ResetStatusIndicator();
    }

    private IEnumerator ProtectGuards()
    {
        foreach (Guard guard in guards)
            guard.IsProtectBonusActivated = true;

        StartEffectTimer(shieldDuration);
        yield return new WaitForSeconds(shieldDuration);

        foreach (Guard guard in guards)
            guard.IsProtectBonusActivated = false;
    }
}
