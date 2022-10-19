using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DifficultyUpdate : MonoBehaviour
{
    [SerializeField, Range(10f, 40f)] private float difficultyIncreaseDelay;

    [SerializeField] private List<Spawner> ufoSpawners;
    [SerializeField] private List<Spawner> meteorSpawners;

    [SerializeField, Range(0.01f, 0.05f)] private float spawnChanceIncrease;
    [SerializeField, Range(0.01f, 0.25f)] private float spawnDelayDecrease;

    private void Start()
    {
        StartCoroutine(IncreaseDifficulty());
    }

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
}
