using System.Collections.Generic;
using UnityEngine;

public class ChargeAllGuards : Bonus
{
    [SerializeField] private List<Guard> guards;
    private int chargeAmount;

    private void Start()
    {
        chargeAmount = (int)UpgradeManager.Instance.CurrChargeEffectValue;
    }

    public void ActivateInstantCharging()
    {
        if (!isBonusEnabled)
            return;

        foreach (Guard guard in guards)
            guard.AddEnergy(chargeAmount);

        isBonusEnabled = false;
    }
}
