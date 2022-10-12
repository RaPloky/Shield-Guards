using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    [SerializeField] int energyAddAmount;

    private Guard _thatStats;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _thatStats = GetComponent<Guard>();
    }

    private void OnMouseDown()
    {
        _thatStats.AddEnergy(energyAddAmount);
        _animator.Play("Touch", -1, 0f);
    }
}
