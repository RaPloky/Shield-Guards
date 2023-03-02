using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    [SerializeField] int energyAddAmount;

    private Guard _thatGuard;

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
