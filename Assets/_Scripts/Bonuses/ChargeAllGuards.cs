using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAllGuards : Bonus
{
    [SerializeField] private List<Guard> guards;
    [SerializeField] private int chargeAmount;

    public void ActivateInstantCharging()
    {
        if (!isBonusEnabled)
            return;

        foreach (Guard guard in guards)
            guard.AddEnergy(chargeAmount);

        isBonusEnabled = false;
        ResetStatusIndicator();
    }
}
