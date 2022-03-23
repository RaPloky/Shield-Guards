using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public float diffIncreaseDelay;
    public float firstDiffIncreaseDelay;
    [Header("Satellites")]
    public int energyUsageIncrement;
    public float dischargeDelay;
    public float dischargeDelayLimit;
    [HideInInspector]
    public LinkedList<GameObject> satellites;
    [Header("UFO")]
    public GameObject UFOPrefab;
    public float attackDelayDecrease;
    public float delayLimit;
    [Header("Meteor")]
    public GameObject MeteorPrefab;
    public int damageIncrease;
    public int damageLimit;
    [Header("Spawner")]
    public float ufoSpawnDelayDecrease;
    public float ufoSpawnDelayLimit;
    public float meteorSpawnDelayDecrease;
    public float meteorSpawnDelayLimit;
    public int spawnChanceIncrease;
    [Header("Bonuses")]
    public GameObject ShieldBonus;
    public int shieldChanceDecrease;
    public GameObject DestroyUfoBonus;
    public int ufoChanceDecrease;
    public GameObject ChargeBonus;
    public int chargeChanceDecrease;

    private UFOBehaviour _UFO;
    private MeteorBehaviour _Meteor;
    private DischargeShieldBonus _Shield;
    private ChargeSatellitesBonus _Charge;
    private DestroyUfosBonus _DestroyUfo;
    private GameObject _Spawner;
    private readonly int _ufoSpawnerIndex = 0;
    private readonly int _meteorSpawnerIndex = 1;

    private void Awake()
    {
        _UFO = UFOPrefab.GetComponent<UFOBehaviour>();
        _Meteor = MeteorPrefab.GetComponent<MeteorBehaviour>();
        _Shield = ShieldBonus.GetComponent<DischargeShieldBonus>();
        _DestroyUfo = DestroyUfoBonus.GetComponent<DestroyUfosBonus>();
        _Charge = ChargeBonus.GetComponent<ChargeSatellitesBonus>();

        satellites = new LinkedList<GameObject>();
        GameObject[] satellitesArray = GameObject.FindGameObjectsWithTag("Satellite");
        foreach (var satellite in satellitesArray)
        {
            satellites.AddLast(satellite);
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(IncreaseDifficulty), firstDiffIncreaseDelay, diffIncreaseDelay);
    }
    public void IncreaseDifficulty()
    {
        IncreaseEnergyDecrement();
        IncreaseUFOAttackFrequency();
        IncreaseMeteorDamage();
        DecreaseEnemiesSpawnDelay();
        DecreaseBonusesInstantiateChance();
        DecreaseDischargeDelay();
    }
    private void DecreaseDischargeDelay()
    {
        foreach (var satellite in satellites)
        {
            if (satellite.GetComponent<GameplayManager>().decrementDelay >= dischargeDelayLimit)
            {
                satellite.GetComponent<GameplayManager>().decrementDelay -= dischargeDelay;
            }
        }
    }
    private void IncreaseEnergyDecrement()
    {
        foreach (var satellite in satellites)
        {
            satellite.GetComponent<GameplayManager>().energyDecrement += energyUsageIncrement;
        }
    }
    private void IncreaseUFOAttackFrequency()
    {
        if (_UFO.attackDelay > delayLimit)
        {
            _UFO.attackDelay -= attackDelayDecrease;
        }
    }
    private void IncreaseMeteorDamage()
    {
        if (_Meteor.damageToSatellite < damageLimit)
        {
            _Meteor.damageToSatellite += damageIncrease;
        }
    }
    private void DecreaseEnemiesSpawnDelay()
    {
        foreach (var satel in satellites)
        {
            _Spawner = satel.transform.GetChild(_ufoSpawnerIndex).gameObject;
            if (_Spawner.GetComponent<EnemySpawnManager>().spawnDelay >= ufoSpawnDelayLimit)
            {
                _Spawner.GetComponent<EnemySpawnManager>().spawnDelay -= ufoSpawnDelayDecrease;
                IncreaseEnemySpawnChance();
            }
            _Spawner = satel.transform.GetChild(_meteorSpawnerIndex).gameObject;
            if (_Spawner.GetComponent<EnemySpawnManager>().spawnDelay >= ufoSpawnDelayLimit)
            {
                _Spawner.GetComponent<EnemySpawnManager>().spawnDelay -= meteorSpawnDelayDecrease;
                IncreaseEnemySpawnChance();
            }
        }
    }
    private void IncreaseEnemySpawnChance()
    {
        EnemySpawnManager manager = _Spawner.GetComponent<EnemySpawnManager>();
        for (int i = 0; i < manager.needToSpawn.Length; i++)
        {
            // Skip "to-instantiate" percents:
            if (manager.needToSpawn[i] == true)
            {
                continue;
            }
            int chanceIncrement = 0;
            for (int j = 1; j <= spawnChanceIncrease; j++)
            {
                manager.needToSpawn[i + j] = true;
                chanceIncrement = j;
            }
            manager.chanceToInstantiate += chanceIncrement;
            break;
        }
    }
    private void DecreaseBonusesInstantiateChance()
    {
        DecreaseChanceOfBonus(_Charge, chargeChanceDecrease);
        DecreaseChanceOfBonus(_Shield, shieldChanceDecrease);
        DecreaseChanceOfBonus(_DestroyUfo, ufoChanceDecrease);
    }
    private void DecreaseChanceOfBonus(BonusManager bonus, int chanceDecrease)
    {
        // 99 is the last accessible index in chances array of lentgh 100:
        for (int i = 99; i >= 0; i--)
        {
            // Skip "not-to-instantiate" chances:
            if (bonus.needToInstantiate[i] == false)
            {
                continue;
            }
            int chanceDecrement = 0;
            for (int j = 1; j <= chanceDecrease; j++)
            {
                bonus.needToInstantiate[i - j] = false;
                chanceDecrement = j;
            }
            bonus.chanceToInstantiate -= chanceDecrement;
            break;
        }
    }
}