using UnityEngine;
public class ChargeSatellitesBonus : BonusManager
{
    [SerializeField] DifficultyManager Manager;
    [SerializeField] GameplayManager satelCharger;
    [SerializeField] int bonusEnergyIncrement;
    [SerializeField] int incrementDebuff;

    private bool _isDebuffed = false;

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
        if (bonusCounter == 0 || PauseMenu.isGamePaused || satelCharger.isDicharged) return;

        foreach (var satel in Manager.satellites)
        {
            int currEnergy = satel.currentEnergyLevel;
            int maxEnergy = satel.maxEnergyLevel;

            if (satel.isDicharged) {
                continue;
            }
            if (currEnergy + bonusEnergyIncrement < maxEnergy) {
                satel.currentEnergyLevel += bonusEnergyIncrement;
            }
            // Case when currEnergy + bonusEnergyIncrement >= maxEnergy:
            else {
                satel.currentEnergyLevel = maxEnergy;
            }
        }
        bonusCounter--;
    }
}
