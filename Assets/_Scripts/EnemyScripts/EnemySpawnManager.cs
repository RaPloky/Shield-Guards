using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] GameplayManager satelliteToDamage;

    public GameObject enemyPrefab;
    public float spawnDelay;
    [Range(0, 100)] public float chanceToInstantiate;
    [HideInInspector]
    public bool enemyIsSpawned = false;
    [HideInInspector]
    public bool spawnFreezed = false;
    [HideInInspector]
    public bool[] needToSpawn;

    private GameplayManager[] _satellitesEnergyComponents;
    private Transform _spawnerTrans;

    private void Awake()
    {
        _spawnerTrans = gameObject.transform;
        // 100% for all chances:
        needToSpawn = new bool[100];

        if (chanceToInstantiate < 0)
        {
            chanceToInstantiate = 0;
        }
        else if (chanceToInstantiate > 100)
        {
            chanceToInstantiate = 100;
        }

        for (byte i = 0; i < chanceToInstantiate; i++)
        {
            needToSpawn[i] = true;
        }
        // Gets satellites energy components to look for game end and stop instant enemy spawn:
        GameObject[] satellites = GameObject.FindGameObjectsWithTag("Satellite");
        int _satellitesCount = satellites.Length;
        _satellitesEnergyComponents = new GameplayManager[_satellitesCount];
        for (byte i = 0; i < _satellitesCount; i++)
        {
            _satellitesEnergyComponents[i] = satellites[i].GetComponent<GameplayManager>();
        }
    }
    private void Start()
    {
        StartSpawn();
    }
    private IEnumerator CheckIfDischarged()
    {
        foreach (var satel in _satellitesEnergyComponents)
        {
            if (satel.isDicharged) yield break;
        }
    }
    public IEnumerator TryToSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        CheckIfDischarged();
        
        // Sets a chance from 0% to 100%:
        int rnd = Random.Range(0, 100);
        if (needToSpawn[rnd] && !enemyIsSpawned && !spawnFreezed)
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
