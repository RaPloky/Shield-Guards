using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    [SerializeField] int energyAddAmount;

    private Stats _thatStats;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _thatStats = GetComponent<Stats>();
    }

    private void OnMouseDown()
    {
        _thatStats.AddEnergy(energyAddAmount);
        _animator.Play("Touch", -1, 0f);
    }
}
