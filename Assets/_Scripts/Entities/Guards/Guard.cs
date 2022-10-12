using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] int energy;
    [SerializeField] int consumption;
    [SerializeField, Range(0.5f, 1f)] float consumptionDelay; 

    private int _maxEnergy;

    public int MaxEnergy => _maxEnergy;
    public int Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Clamp(value, 0, _maxEnergy);
            EventManager.SendOnEnergyValueChanged();

            if (Mathf.Approximately(energy, 0))
                TurnOffGuard();
        }
    }

    private void Awake()
    {
        _maxEnergy = energy;
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
    }

    public void AddEnergy(int energyAmount) => Energy += energyAmount;
    public void ConsumptEnergy(int energyConsumption) => Energy -= energyConsumption;
}
