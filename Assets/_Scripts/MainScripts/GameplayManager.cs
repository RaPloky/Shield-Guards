﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [Header("Gameplay")]
    public DifficultyManager DifficultyManager;
    public int energyIncrement, energyDecrement;
    public int maxEnergyLevel, minEnergyLevel = 0, currentEnergyLevel;
    public float decrementDelay;
    [HideInInspector]
    public bool isDicharged = false;
    [Header("Score")]
    public ScoreCounter ScoreCounter;
    public HighScoreCounter HighScoreCounter;
    [Header("Bonuses")]
    public ChargeSatellitesBonus ChargeBonus;
    public DischargeShieldBonus DischargeShieldBonus;
    [Header("UI")]
    public PauseMenu Canvas;
    [Header("Animations")]
    [SerializeField] Animation animations;

    private readonly int _chargeBonusCooldown = 10;
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
        if (isDicharged || PauseMenu.isGamePaused) return;
        AddScore();
        ChargeSatellite();

        if (animations.IsPlaying(animations.name))
        {
            animations.Stop();
        }
        animations.Play();
    }
    private void AddScore()
    {
        // Multipling on satellites count is neccessary for game design -
        // it shall to motivate player to hold ALL satellites with non-zero
        // energy. Score with 3 satellites will be much more bigger than score
        // with 1 or 2 satellites:
        ScoreCounter.currentScore += energyIncrement * ScoreCounter.scoreMultiplier * DifficultyManager.satellites.Count;
    }
    private void PowerOffSatellite()
    {
        DifficultyManager.satellites.Remove(gameObject);
        if (Mathf.Approximately(DifficultyManager.satellites.Count, 0))
        {
            Canvas.LoadLoseMenu();
        }
    }
    private IEnumerator ConstantEnergyDecrement()
    {
        yield return new WaitForSeconds(decrementDelay);

        // Stops coroutine after lose:
        if (currentEnergyLevel <= minEnergyLevel && isDicharged)
        {
            yield break;
        }
        currentEnergyLevel = Mathf.Clamp(currentEnergyLevel - energyDecrement, minEnergyLevel, maxEnergyLevel);
        StartCoroutine(ConstantEnergyDecrement());
    }
    private void ChargeSatellite()
    {
        // Lets to charge energy until max energy level will be reached:
        if (currentEnergyLevel < maxEnergyLevel)
        {
            currentEnergyLevel = Mathf.Clamp(currentEnergyLevel + energyIncrement, minEnergyLevel, maxEnergyLevel);
        }
        TryToInstantiateChargeSatellitesBonus();
    }
    private void TryToInstantiateChargeSatellitesBonus()
    {
        if (_enteredChargeBonusCooldown) return;

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