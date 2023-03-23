using System.Collections.Generic;
using UnityEngine;

public class ChargeAllGuards : Bonus
{
    [SerializeField] private List<Guard> guards;
    private int chargeAmount;

    private DifficultyUpdate _difficultyManager;

    private void Start()
    {
        _difficultyManager = DifficultyUpdate.Instance;
        chargeAmount = (int)UpgradeManager.Instance.CurrChargeEffectValue;
    }

    public void ActivateBonus()
    {
        if (!isBonusEnabled)
            return;

        foreach (Guard activeGuard in _difficultyManager.ActiveGuards)
            activeGuard.AddEnergy(chargeAmount);

        ChangeActivationButtonStatus(false);
        isBonusEnabled = false;
    }
}
