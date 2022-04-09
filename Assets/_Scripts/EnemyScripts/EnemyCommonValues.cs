using UnityEngine;

abstract public class EnemyCommonValues : MonoBehaviour
{
    [HideInInspector] public Transform spawnManagerTrans;
    [HideInInspector] public GameObject satelliteToDamage;
    public int damageToSatellite;
    public int destroyReward;

    protected ScoreCounter _scoreCounter;
    protected GameplayManager _satelliteEnergy;

    private readonly int _childOrderInHierarchy = 0;

    public void DoDamage(GameplayManager satel)
    {
        satel.currentEnergyLevel = Mathf.Clamp(satel.currentEnergyLevel - damageToSatellite, satel.minEnergyLevel, satel.maxEnergyLevel);
    }
    public void DestroyAndStartSpawn(Transform spawnManagerTrans)
    {
        Destroy(spawnManagerTrans.GetChild(_childOrderInHierarchy).gameObject);
        // Enabling to start spawn coroutine:
        spawnManagerTrans.gameObject.GetComponent<EnemySpawnManager>().enemyIsSpawned = false;
        spawnManagerTrans.gameObject.GetComponent<EnemySpawnManager>().StartSpawn();
    }
    public void SetOnAwake()
    {
        _scoreCounter = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreCounter>();
        satelliteToDamage = spawnManagerTrans.parent.gameObject;
        _satelliteEnergy = satelliteToDamage.GetComponent<GameplayManager>();
    }
    public void AddScore()
    {
        _scoreCounter.currentScore += destroyReward * _scoreCounter.scoreMultiplier;
    }
}
