﻿using System.Collections;
using UnityEngine;

public class SatelliteBehavior : MonoBehaviour
{
    [SerializeField] DifficultyManager DifficultyManager;
    public float energyIncrement, energyDecrement;
    public float maxEnergyLevel, minEnergyLevel = 0, currentEnergyLevel;
    public float decrementDelay;
    [HideInInspector]
    public bool isDicharged = false;
    public ChargeSatellitesBonus ChargeBonus;
    [SerializeField] PauseMenu GameOverMenu;

    private const int ROUND_DECIMALS = 2;
    private int _chargeBonusCooldown = 10;
    private bool _enteredChargeBonusCooldown = false;

    private void Start()
    {
        StartCoroutine(ConstantEnergyDecrement());
    }
    private void Update()
    {
        // Condition to lose the satellite:
        if (currentEnergyLevel <= minEnergyLevel && !isDicharged)
        {
            PowerOffSatellite();
            isDicharged = true;
        }
    }
    private void OnMouseDown()
    {
        if (isDicharged || PauseMenu.isGamePaused) 
            return;
        ChargeSatellite();
    }
    private void PowerOffSatellite()
    {
        DifficultyManager.activeSatellites.Remove(gameObject);
        if (Mathf.Approximately(DifficultyManager.activeSatellites.Count, 0))
        {
            EndGame();
        }
    }
    private void EndGame()
    {
        GameOverMenu.LoadLoseMenu();
    }
    private IEnumerator ConstantEnergyDecrement()
    {
        while (!(currentEnergyLevel <= minEnergyLevel && isDicharged))
        {
            yield return new WaitForSeconds(decrementDelay);
            currentEnergyLevel = (float)System.Math.Round(currentEnergyLevel, ROUND_DECIMALS);

            currentEnergyLevel = Mathf.Clamp(currentEnergyLevel - energyDecrement, minEnergyLevel, maxEnergyLevel);
        }
    }
    private void ChargeSatellite()
    {
        // Lets to charge energy until max energy level will be reached:
        if (currentEnergyLevel < maxEnergyLevel)
        {
            currentEnergyLevel = Mathf.Clamp(currentEnergyLevel + energyIncrement, minEnergyLevel, maxEnergyLevel);
        }
        TryToInstantiateChargeBonus();
    }
    private void TryToInstantiateChargeBonus()
    {
        if (_enteredChargeBonusCooldown) 
            return;

        if (Mathf.Approximately(currentEnergyLevel, maxEnergyLevel))
        {
            ChargeBonus.InstantiateBonus();
            _enteredChargeBonusCooldown = true;
            // Rejects attempts to instantiate charge bonus
            // many times in a short time range:
            Invoke(nameof(ExitChargeCoolDown), _chargeBonusCooldown);
        }
    }
    private void ExitChargeCoolDown()
    {
        _enteredChargeBonusCooldown = false;
    }
}