using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ufoPrefab;
    [SerializeField] private float spawnDelay;
    [SerializeField, Range(0f, 1f)] private float spawnChance;

    private GameObject _activeUfo;

    private void Start()
    {
        StartCoroutine(SpawnUfo());        
    }

    private IEnumerator SpawnUfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (_activeUfo == null && IsSpawnAllowed())
                _activeUfo = Instantiate(ufoPrefab, transform);
        }
    }

    private bool IsSpawnAllowed()
    {
        float randomChance = Random.Range(0f, 1f);
        return randomChance < spawnChance;
    }
}
