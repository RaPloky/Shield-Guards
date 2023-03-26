using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private int energy;
    [SerializeField] private int consumption;
    [SerializeField, Range(0.5f, 1f)] private float consumptionDelay;
    [SerializeField] private Bonus relatedBonus;
    [SerializeField] private CanvasGroup relatedDangerNotifications;

    private int _maxEnergy;
    private bool _isHaveEnergy;
    private Guard _thatGuard;
    private float _startConsumptionDelay;

    public int MaxEnergy => _maxEnergy;
    public int Energy
    {
        get => energy;
        set
        {
            if (IsProtectBonusActivated)
                return;

            energy = Mathf.Clamp(value, 0, _maxEnergy);
            EventManager.SendOnEnergyValueChanged();

            if (Mathf.Approximately(energy, 0) && _isHaveEnergy)
                TurnOffGuard();
        }
    }
    public bool IsHaveEnergy => _isHaveEnergy;
    public bool IsProtectBonusActivated { get; set; }
    public Transform RelatedBonus => relatedBonus.transform;
    public float ConsumptionDelay
    {
        get => consumptionDelay;
        set => consumptionDelay = value;
    }
    public float StartConsumptionDelay => _startConsumptionDelay;
    public CanvasGroup RelatedDangerNotifications => relatedDangerNotifications;

    private void Awake()
    {
        IsProtectBonusActivated = false;
        _maxEnergy = energy;
        _isHaveEnergy = true;
        StartCoroutine(ConsumptEnergy());
        _thatGuard = this;
        _startConsumptionDelay = ConsumptionDelay;
    }

    private IEnumerator ConsumptEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(consumptionDelay);
            ConsumptEnergy(consumption);
        }
    }

    private void TurnOffGuard()
    {
        StopAllCoroutines();
        GetComponent<BoxCollider>().enabled = false;
        _isHaveEnergy = false;
        relatedBonus.DisableBonus();
        DifficultyUpdate.Instance.RemoveGuardFromList(ref _thatGuard);
        EventManager.SendOnGuardDischarged();
    }

    public void AddEnergy(int energyAmount) => Energy += energyAmount;
    public void ConsumptEnergy(int energyConsumption) => Energy -= energyConsumption;
}
