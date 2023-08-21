using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    [SerializeField] private int energyAddAmount;
    [SerializeField] private AudioClip energyAddSound;
    [SerializeField] private PlayOneShotSound soundSource;

    private Guard _thatGuard;
    private GameManager _gameManager;
    private const int _scoreCutter = 4;

    public int EnergyAddAmount
    {
        get => energyAddAmount;
        set => energyAddAmount = value;
    }

    private void Awake()
    {
        _thatGuard = GetComponent<Guard>();
        _gameManager = GameManager.Instance;
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused || !_thatGuard.IsHaveEnergy)
            return;

        _thatGuard.AddEnergy(energyAddAmount);
        EventManager.SendOnNonBonusEnergyAdded();
        EventManager.SendOnScoreUpdated((energyAddAmount / _scoreCutter) * _gameManager.ActiveGuardsCount);

        soundSource.PlayClip(energyAddSound);
    }
}
