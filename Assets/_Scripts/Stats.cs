using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] int energy;
    [SerializeField] int consumption;
    [SerializeField, Range(0.5f, 1f)] float consumptionDelay; 

    private int _maxEnergy;

    private void Start()
    {
        _maxEnergy = energy;
        StartCoroutine(ConsumptEnergy());
    }

    public int Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Clamp(value, 0, _maxEnergy);
            if (Mathf.Approximately(energy, 0))
                TurnOffSatellite();
        }
    }

    public void AddEnergy(int energyAmount) => Energy += energyAmount;
    public void ConsumptEnergy(int energyConsumption) => Energy -= energyConsumption;

    private IEnumerator ConsumptEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(consumptionDelay);
            ConsumptEnergy(consumption);
        }
    }

    private void TurnOffSatellite()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        StopAllCoroutines();
    }
}
