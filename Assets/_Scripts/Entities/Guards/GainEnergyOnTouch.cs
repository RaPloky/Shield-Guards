using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    [SerializeField] int energyAddAmount;

    private Guard _thatGuard;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _thatGuard = GetComponent<Guard>();
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused)
            return;

        _thatGuard.AddEnergy(energyAddAmount);
        _animator.Play("Touch", -1, 0f);
        EventManager.SendOnNonBonusEnergyAdded();
        EventManager.SendOnScoreUpdated(energyAddAmount);
    }
}
