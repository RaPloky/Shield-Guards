using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAllGuards : MonoBehaviour
{
    [SerializeField] private List<Guard> guards;
    [SerializeField] private int chargeAmount;
    [SerializeField] private bool isBonusEnabled;

    public void ActivateInstantCharging()
    {
        if (!isBonusEnabled)
            return;

        foreach (Guard guard in guards)
        {
            guard.AddEnergy(chargeAmount);
        }
        isBonusEnabled = false;
    }
}
