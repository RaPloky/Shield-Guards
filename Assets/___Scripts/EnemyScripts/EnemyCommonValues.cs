using UnityEngine;

abstract public class EnemyCommonValues : MonoBehaviour
{
    public Transform spawnManagerTrans;
    public GameObject satelliteToDamage;
    public int damageToSatellite;
    public int scoreReward;

    protected ScoreCounter _scoreCounter;
    protected GameplayManager _satelliteEnergy;

    private readonly int _childOrderInHierarchy = 0;

    public void DoDamage(GameObject satellite)
    {
        GameplayManager satel = satellite.GetComponent<GameplayManager>();
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
        satelliteToDamage = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        _satelliteEnergy = satelliteToDamage.GetComponent<GameplayManager>();
    }
    public void AddScore()
    {
        _scoreCounter.currentScore += scoreReward * _scoreCounter.scoreMultiplier;
    }
}
