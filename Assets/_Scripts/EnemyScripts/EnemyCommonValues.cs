using UnityEngine;

abstract public class EnemyCommonValues : MonoBehaviour
{
    public float damageToSatellite;
    public int destroyReward;

    protected SatelliteBehavior _satelliteEnergy;
    protected GameObject _satelliteToDamage;
    protected Transform _spawnManagerTrans;

    private const int _childOrderInHierarchy = 0;

    public void DestroyAndStartSpawn(Transform spawnerTrans)
    {
        Destroy(spawnerTrans.GetChild(_childOrderInHierarchy).gameObject);
        // Enabling to start spawn coroutine:
        spawnerTrans.gameObject.GetComponent<EnemySpawnManager>().enemyIsSpawned = false;
        spawnerTrans.gameObject.GetComponent<EnemySpawnManager>().StartSpawn();
    }
    protected void DoDamage(SatelliteBehavior satel)
    {
        satel.currentEnergyLevel = Mathf.Clamp(satel.currentEnergyLevel - damageToSatellite, satel.minEnergyLevel, satel.maxEnergyLevel);
    }
    protected void SetOnAwake()
    {
        _satelliteToDamage = _spawnManagerTrans.gameObject.GetComponent<EnemySpawnManager>().target.gameObject;
        _satelliteEnergy = _satelliteToDamage.GetComponent<SatelliteBehavior>();
    }
    protected void AddScore()
    {
        ScoreCounter.currentScore += destroyReward;
        UpdateScore();
    }
    private void UpdateScore()
    {
        ScoreCounter.scoreText.text = ScoreCounter.currentScore.ToString();
    }
}
