using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameplayManager satelliteToDamage;
    public GameObject enemyPrefab;
    public float spawnDelay;
    [Range(0, 100)] 
    public float chanceToInstantiate;
    [HideInInspector]
    public bool enemyIsSpawned = false;
    [HideInInspector]
    public bool spawnFreezed = false;
    [HideInInspector]
    public bool[] spawnChance;

    private Transform _spawnerTrans;

    private void Awake()
    {
        _spawnerTrans = gameObject.transform;
        // 100% for all chances:
        spawnChance = new bool[100];

        for (byte i = 0; i < chanceToInstantiate; i++)
        {
            spawnChance[i] = true;
        }
    }
    private void Start()
    {
        StartSpawn();
    }
    private bool CheckIfDischarged()
    {
        bool isDischarged = false;
        if (satelliteToDamage.isDicharged)
        {
            isDischarged = true;
        }
        return isDischarged;
    }
    public IEnumerator TryToSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (CheckIfDischarged()) yield break;
        
        // Sets a chance from 0% to 100%:
        int rnd = Random.Range(0, 100);
        if (spawnChance[rnd] && !enemyIsSpawned && !spawnFreezed)
        {
            enemyIsSpawned = true;
            Instantiate(enemyPrefab, _spawnerTrans);
        }
        // If enemy spawns, coroutine must have
        // to stop working while enemy is active:
        if (!enemyIsSpawned) 
        {
            StartCoroutine(TryToSpawn()); 
        }
    }
    public void StartSpawn()
    {
        StartCoroutine(TryToSpawn());
    }
}
