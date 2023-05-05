using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DifficultyUpdate : MonoBehaviour
{
    public static DifficultyUpdate Instance;

    [SerializeField, Range(10f, 40f)] private float difficultyIncreaseDelay;

    [SerializeField] private List<Spawner> ufoSpawners;
    [SerializeField] private List<Spawner> meteorSpawners;

    [SerializeField, Range(0.01f, 0.05f)] private float spawnChanceIncrease;
    [SerializeField, Range(0.01f, 0.25f)] private float spawnDelayDecrease;
    [SerializeField, Range(0, 0.25f)] private float consumptionDelayDecrease;
    [SerializeField, Range(0.2f, 0.5f)] private float consumptionDelayLimit; 

    [SerializeField] private List<Guard> activeGuards;

    public List<Guard> ActiveGuards => activeGuards;

    private WaitForSeconds DifficultyIncreaseDelay;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(IncreaseDifficulty());
        DifficultyIncreaseDelay = new WaitForSeconds(difficultyIncreaseDelay);
    }

    private void OnEnable() => EventManager.OnGuardDischarged += IncreaseEnergyConsumption;
    private void OnDisable() => EventManager.OnGuardDischarged -= IncreaseEnergyConsumption;

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return DifficultyIncreaseDelay;

            // message about difficulty increase

            UpdateSpawnersStats(ufoSpawners);
            UpdateSpawnersStats(meteorSpawners);
        }
    }

    private void UpdateSpawnersStats(List<Spawner> spawners)
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].LaunchChance += spawnChanceIncrease;
            spawners[i].SpawnDelay -= spawnDelayDecrease;
        }
    }

    public void RemoveGuardFromList(ref Guard guard) => activeGuards.Remove(guard);

    private void IncreaseEnergyConsumption()
    {
        for (int i = 0; i < activeGuards.Count; i++)
        {
            activeGuards[i].ConsumptionDelay = Mathf.Clamp(activeGuards[i].ConsumptionDelay - consumptionDelayDecrease, 
                consumptionDelayLimit, activeGuards[i].StartConsumptionDelay);
        }
    }
}
