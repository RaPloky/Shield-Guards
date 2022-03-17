using System.Collections.Generic;

public class DischargeShieldBonus : BonusManager
{
    public DifficultyManager Manager;
    public GameplayManager satelSupport;
    public int bonusDuration;
    public int criticalEnergyLevel;
    public int maxTimeBeingInCriticalZone = 10;
    public int decrementBuff;

    private List<GameplayManager> _energyComponents;
    private List<int> _defaultEnergyDecrements;
    private readonly int _invokeDelay = 1;
    private int _secondsInCriticalZone;
    private bool _enteredCriticalEnergyZone = false;
    private bool _isDebuffed = false;

    private void Awake()
    {
        _defaultEnergyDecrements = new List<int>();
        _energyComponents = new List<GameplayManager>();
        SetChanceToInstantiate();
        foreach (var satel in Manager.satellites)
        {
            _defaultEnergyDecrements.Add(satel.GetComponent<GameplayManager>().energyDecrement);
            _energyComponents.Add(satel.GetComponent<GameplayManager>());
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
        foreach (var satel in _energyComponents)
        {
            if (satel.currentEnergyLevel <= criticalEnergyLevel)
                _enteredCriticalEnergyZone = true;
            else
                _enteredCriticalEnergyZone = false;
        }

        if (satelSupport.isDicharged && !_isDebuffed)
        {
            _isDebuffed = true;
            BuffEnergyDecrement();
            return;
        }
    }
    private void BuffEnergyDecrement()
    {
        foreach (var satel in Manager.satellites)
        {
            satel.GetComponent<GameplayManager>().energyDecrement += decrementBuff;
        }
    }
    public override void ActivateBonus()
    {
        if (bonusCounter == 0 || PauseMenu.isGamePaused || satelSupport.isDicharged) return;

        // "0" bcz number -= 0 is equal to number - energy stay unchanged:
        SetNewEnergyDecrementValue(new int[] { 0, 0, 0 });
        Invoke(nameof(RestoreEnergyDecrement), bonusDuration);
        bonusCounter--;
    }
    private void RestoreEnergyDecrement()
    {
        SetNewEnergyDecrementValue(_defaultEnergyDecrements.ToArray());
    }
    private void SetNewEnergyDecrementValue(int[] newValues)
    {
        int i = 0;
        foreach (var satel in _energyComponents)
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
