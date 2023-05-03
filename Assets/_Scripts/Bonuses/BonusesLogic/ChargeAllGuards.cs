using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargeAllGuards : Bonus
{
    [SerializeField] private List<Guard> guards;
    [SerializeField] private Animator hudAnimatorOnBonusActivation;
    [SerializeField] private TextMeshProUGUI bonusActivationText;
    private int chargeAmount;

    private DifficultyUpdate _difficultyManager;

    private void Start()
    {
        _difficultyManager = DifficultyUpdate.Instance;
        chargeAmount = (int)UpgradeManager.Instance.CurrChargeEffectValue;
        bonusActivationText.text = $"+{chargeAmount}";
    }

    public void ActivateBonus()
    {
        if (!isBonusEnabled)
            return;

        for (int i = 0; i < _difficultyManager.ActiveGuards.Count; i++)
            _difficultyManager.ActiveGuards[i].AddEnergy(chargeAmount);

        ActivateAnimation();
        PlayActivationParticleSystem();

        ChangeActivationButtonStatus(false);
        isBonusEnabled = false;
    }

    private void ActivateAnimation()
    {
        hudAnimatorOnBonusActivation.SetTrigger("BonusActivated");
    }
}
