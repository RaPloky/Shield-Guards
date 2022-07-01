﻿using UnityEngine;
public class ChargeSatellitesBonus : BonusManager
{
    [SerializeField] DifficultyManager Manager;
    [SerializeField] SatelliteBehavior satelCharger;
    [SerializeField] int bonusEnergyIncrement;
    [SerializeField] int incrementDebuff;

    private bool _isDebuffed = false;

    private void Awake()
    {
        bonusCountUI.text = "x" + bonusCount.ToString();
    }
    private void Update()
    {
        if (satelCharger.isDicharged && !_isDebuffed)
        {
            _isDebuffed = true;
            DebuffEnergyIncrement();
            return;
        }
    }
    private void DebuffEnergyIncrement()
    {
        foreach (var satel in Manager.satellites)
        {
            satel.energyIncrement -= incrementDebuff;
        }
    }
    public override void ActivateBonus()
    {
        if (bonusCount == 0 || PauseMenu.isGamePaused || satelCharger.isDicharged) 
            return;

        foreach (var satel in Manager.satellites)
        {
            if (satel.isDicharged) 
                continue;

            satel.currentEnergyLevel = Mathf.Clamp(satel.currentEnergyLevel + bonusEnergyIncrement, 
                satel.minEnergyLevel, satel.maxEnergyLevel);            
        }
        UpdateBonusCount();
    }
}
