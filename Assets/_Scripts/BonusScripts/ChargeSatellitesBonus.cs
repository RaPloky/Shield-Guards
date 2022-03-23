public class ChargeSatellitesBonus : BonusManager
{
    public DifficultyManager Manager;
    public GameplayManager satelCharger;
    public int bonusEnergyIncrement;
    public int incrementDebuff;

    private bool _isDebuffed = false;

    private void Awake()
    {
        SetChanceToInstantiate();
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
            satel.GetComponent<GameplayManager>().energyIncrement -= incrementDebuff;
        }
    }
    public override void ActivateBonus()
    {
        if (bonusCounter == 0 || PauseMenu.isGamePaused || satelCharger.isDicharged) return;

        foreach (var satel in Manager.satellites)
        {
            int currEnergy = satel.GetComponent<GameplayManager>().currentEnergyLevel;
            int maxEnergy = satel.GetComponent<GameplayManager>().maxEnergyLevel;

            if (currEnergy + bonusEnergyIncrement < maxEnergy)
                satel.GetComponent<GameplayManager>().currentEnergyLevel += bonusEnergyIncrement;
            // In case, when bonus energy can't be added bcz energy limit, it charges to maximum:
            else
                satel.GetComponent<GameplayManager>().currentEnergyLevel = maxEnergy;
        }
        bonusCounter--;
    }
}
