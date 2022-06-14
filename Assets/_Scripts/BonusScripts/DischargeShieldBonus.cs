using System.Collections.Generic;
using UnityEngine;

public class DischargeShieldBonus : BonusManager
{
    [SerializeField] DifficultyManager DifficultyManager;
    [SerializeField] GameplayManager satelSupport;
    [SerializeField] List<GameplayManager> energyComponents;
    [SerializeField] int bonusDuration;
    [SerializeField] int maxTimeBeingInCriticalZone = 10;
    [SerializeField] int decrementBuff;
    public int criticalEnergyLevel;
    
    private List<float> _defaultEnergyDecrements;
    private readonly int _invokeDelay = 1;
    private int _secondsInCriticalZone;
    private bool _enteredCriticalEnergyZone = false;
    private bool _isEnergyDecrementBuffed = false;

    private void Awake()
    {
        _defaultEnergyDecrements = new List<float>();
        foreach (var satel in DifficultyManager.satellites)
        {
            _defaultEnergyDecrements.Add(satel.energyDecrement);
        }
    }
    private void Start()
    {
        // "1" because method call is need literally right after the game starts:
        InvokeRepeating(nameof(CountSecondsInCriticalZone), 1, _invokeDelay);
    }
    private void Update()
    {
        // Looking for critical energy level on any of satellites:
        foreach (var satel in energyComponents)
        {
            if (satel.currentEnergyLevel <= criticalEnergyLevel)
                _enteredCriticalEnergyZone = true;
            else
                _enteredCriticalEnergyZone = false;
        }

        if (satelSupport.isDicharged && !_isEnergyDecrementBuffed)
        {
            _isEnergyDecrementBuffed = true;
            BuffEnergyDecrement();
            return;
        }
    }
    private void BuffEnergyDecrement()
    {
        foreach (var satel in DifficultyManager.satellites)
        {
            satel.energyDecrement += decrementBuff;
        }
    }
    public override void ActivateBonus()
    {
        if (bonusCounter == 0 || PauseMenu.isGamePaused || satelSupport.isDicharged) return;

        // "0" bcz number -= 0 is equal to number - energy stay unchanged:
        SetNewEnergyDecrementValue(new float[] { 0f, 0f, 0f });
        Invoke(nameof(RestoreEnergyDecrement), bonusDuration);
        bonusCounter--;
    }
    private void RestoreEnergyDecrement()
    {
        SetNewEnergyDecrementValue(_defaultEnergyDecrements.ToArray());
    }
    private void SetNewEnergyDecrementValue(float[] newValues)
    {
        int i = 0;
        foreach (var satel in energyComponents)
        {
            satel.energyDecrement = newValues[i];
            i++;
        }
    }
    private void CountSecondsInCriticalZone()
    {
        if (!_enteredCriticalEnergyZone) return;
        _secondsInCriticalZone++;
        TryToInstantiateDischargeShieldBonus();
    }
    private void TryToInstantiateDischargeShieldBonus()
    {
        if (_secondsInCriticalZone >= maxTimeBeingInCriticalZone)
        {
            InstantiateBonus();
            // Resets timer every instantiate attempt:
            _secondsInCriticalZone = 0; 
        }
    }
}
