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

    [SerializeField] private List<Guard> activeGuards;

    private void Start()
    {
        Instance = this;
        StartCoroutine(IncreaseDifficulty());
    }

    private void OnEnable() => EventManager.OnGuardDischarged += IncreaseEnergyConsumption;
    private void OnDisable() => EventManager.OnGuardDischarged -= IncreaseEnergyConsumption;

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseDelay);

            // message about difficulty increase

            UpdateSpawnersStats(ufoSpawners);
            UpdateSpawnersStats(meteorSpawners);
        }
    }

    private void UpdateSpawnersStats(List<Spawner> spawners)
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.LaunchChance += spawnChanceIncrease;
            spawner.SpawnDelay -= spawnDelayDecrease;
        }
    }

    public void RemoveGuardFromList(ref Guard guard) => activeGuards.Remove(guard);

    private void IncreaseEnergyConsumption()
    {
        foreach (Guard guard in activeGuards)
            guard.ConsumptionDelay -= consumptionDelayDecrease;
    }
}
