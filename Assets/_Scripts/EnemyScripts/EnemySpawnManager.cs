using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public SatelliteBehavior target;
    public GameObject enemyPrefab;
    [Range(3f, 15f)]
    public float spawnDelay;
    [Range(0, 1)] 
    public float chanceToInstantiate;
    [HideInInspector]
    public bool enemyIsSpawned = false;
    [HideInInspector]
    public bool spawnFreezed = false;

    private Transform _spawnerTrans;

    private void Awake()
    {
        _spawnerTrans = gameObject.transform;
    }
    private void Start()
    {
        StartSpawn();
    }
    private bool CheckIfDischarged()
    {
        bool isDischarged = false;
        if (target.isDicharged)
            isDischarged = true;
        return isDischarged;
    }
    private IEnumerator TryToSpawn()
    {
        // If enemy spawns, coroutine must have
        // to stop working while enemy is active:
        while (!enemyIsSpawned)
        {
            float spawnRandDelay = Random.Range(0f, 1f);
            yield return new WaitForSeconds(spawnDelay + spawnRandDelay);

            if (CheckIfDischarged()) 
                yield break;

            float rnd = Random.Range(0f, 1f);
            if (rnd < chanceToInstantiate && !enemyIsSpawned && !spawnFreezed)
            {
                enemyIsSpawned = true;
                Instantiate(enemyPrefab, _spawnerTrans);
            }
        }
    }
    public void StartSpawn()
    {
        StartCoroutine(TryToSpawn());
    }
}
