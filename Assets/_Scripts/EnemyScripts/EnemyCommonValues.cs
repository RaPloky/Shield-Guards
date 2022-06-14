using UnityEngine;

abstract public class EnemyCommonValues : MonoBehaviour
{
    public float damageToSatellite;
    public int destroyReward;

    protected GameplayManager _satelliteEnergy;
    protected GameObject _satelliteToDamage;
    protected Transform _spawnManagerTrans;
    protected ScoreCounter _scoreCounter;

    private readonly int _childOrderInHierarchy = 0;

    public void DoDamage(GameplayManager satel)
    {
        satel.currentEnergyLevel = Mathf.Clamp(satel.currentEnergyLevel - damageToSatellite, satel.minEnergyLevel, satel.maxEnergyLevel);
    }
    public void DestroyAndStartSpawn(Transform spawnerTrans)
    {
        Destroy(spawnerTrans.GetChild(_childOrderInHierarchy).gameObject);
        // Enabling to start spawn coroutine:
        spawnerTrans.gameObject.GetComponent<EnemySpawnManager>().enemyIsSpawned = false;
        spawnerTrans.gameObject.GetComponent<EnemySpawnManager>().StartSpawn();
    }
    public void SetOnAwake()
    {
        _scoreCounter = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreCounter>();
        _satelliteToDamage = _spawnManagerTrans.gameObject.GetComponent<EnemySpawnManager>().satelliteToDamage.gameObject;
        _satelliteEnergy = _satelliteToDamage.GetComponent<GameplayManager>();
    }
    public void AddScore()
    {
        _scoreCounter.currentScore += destroyReward * _scoreCounter.scoreMultiplier;
    }
}
