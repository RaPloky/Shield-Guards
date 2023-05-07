using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    [SerializeField] private int energyAddAmount;

    private Guard _thatGuard;

    public int EnergyAddAmount
    {
        get => energyAddAmount;
        set => energyAddAmount = value;
    }

    private void Awake()
    {
        _thatGuard = GetComponent<Guard>();
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused || !_thatGuard.IsHaveEnergy)
            return;

        _thatGuard.AddEnergy(energyAddAmount);
        EventManager.SendOnNonBonusEnergyAdded();
        EventManager.SendOnScoreUpdated(energyAddAmount);
    }
}
