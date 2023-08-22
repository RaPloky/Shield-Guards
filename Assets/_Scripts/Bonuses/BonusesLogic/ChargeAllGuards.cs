using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        for (int i = 0; i < _difficultyManager.ActiveGuards.Count; i++)
            _difficultyManager.ActiveGuards[i].AddEnergy(chargeAmount);

        PlaySound(activationSound);
        PlayActivationParticleSystem();

        ChangeActivationButtonStatus(false);
        isBonusEnabled = false;
    }
}
