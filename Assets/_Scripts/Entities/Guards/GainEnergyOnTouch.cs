using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    [SerializeField] private int energyAddAmount;
    [SerializeField] private AudioClip energyAddSound;
    
    private AudioSource _soundSource;
    private Guard _thatGuard;
    private GameManager _gameManager;
    private const int _scoreCutter = 4;

    public int EnergyAddAmount
    {
        get => energyAddAmount;
        set => energyAddAmount = value;
    }

    private void Start()
    {
        _thatGuard = GetComponent<Guard>();
        _gameManager = GameManager.Instance;
        _soundSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused || !_thatGuard.IsHaveEnergy)
            return;

        if (Mathf.Approximately(_thatGuard.Energy, _thatGuard.MaxEnergy))
            return;

        _thatGuard.AddEnergy(energyAddAmount);
        EventManager.SendOnNonBonusEnergyAdded();
        EventManager.SendOnScoreUpdated((energyAddAmount / _scoreCutter) * _gameManager.ActiveGuardsCount);

        _soundSource.PlayOneShot(energyAddSound);
    }
}
