using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectAllGuards : Bonus
{
    [Header("Resist bonus")]
    [SerializeField] private List<Guard> guards;
    [SerializeField] private List<GameObject> guardShields;
    private float shieldDuration;

    private void Start()
    {
        shieldDuration = UpgradeManager.Instance.CurrProtectionEffectValue;
    }

    public void ActivateBonus()
    {
        if (!isBonusEnabled)
            return;

        effectDurationGO.SetActive(true);
        StartCoroutine(ProtectGuards());

        ChangeActivationButtonStatus(false);
        isBonusEnabled = false;
    }

    private IEnumerator ProtectGuards()
    {
        for (int i = 0; i < guards.Count; i++)
            guards[i].IsProtectBonusActivated = true;

        StartEffectTimer(shieldDuration);
        ReactivateShields(true);

        yield return new WaitForSeconds(shieldDuration);

        for (int i = 0; i < guards.Count; i++)
            guards[i].IsProtectBonusActivated = false;

        ReactivateShields(false);
    }

    private void ReactivateShields(bool condition)
    {
        for (int i = 0; i < guardShields.Count; i++)
            guardShields[i].SetActive(condition);
    }
}
