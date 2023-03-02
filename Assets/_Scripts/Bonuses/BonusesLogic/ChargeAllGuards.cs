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

    public void ActivateBonus()
    {
        if (!isBonusEnabled)
            return;

        foreach (Guard guard in guards)
            guard.AddEnergy(chargeAmount);

        ChangeActivationButtonStatus(false);
        isBonusEnabled = false;
    }
}
