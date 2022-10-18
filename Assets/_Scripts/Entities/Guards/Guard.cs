using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private int energy;
    [SerializeField] private int consumption;
    [SerializeField, Range(0.5f, 1f)] private float consumptionDelay;
    [SerializeField] private Bonus relatedBonus;

    private int _maxEnergy;
    private bool _isHaveEnergy;

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

            if (Mathf.Approximately(energy, 0))
                TurnOffGuard();
        }
    }
    public bool IsHaveEnergy => _isHaveEnergy;
    public bool IsProtectBonusActivated { get; set; }
    public Transform RelatedBonus => relatedBonus.transform;

    private void Awake()
    {
        IsProtectBonusActivated = false;
        _maxEnergy = energy;
        _isHaveEnergy = true;
        StartCoroutine(ConsumptEnergy());
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
        // Other cool destroy stuff
        GetComponent<Animator>().enabled = false;
        StopAllCoroutines();
        _isHaveEnergy = false;
        relatedBonus.DisableBonusButton();
    }

    public void AddEnergy(int energyAmount) => Energy += energyAmount;
    public void ConsumptEnergy(int energyConsumption) => Energy -= energyConsumption;
}
