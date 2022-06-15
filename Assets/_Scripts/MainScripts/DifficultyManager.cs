using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] float diffIncreaseDelay;
    [SerializeField] float firstDiffIncreaseDelay;
    #region "Satellites"
    [Header("Satellites")]
    [Range(0f, 1.5f)]
    [SerializeField] float energyUsageIncrement;
    [SerializeField] float dischargeDelay;
    [SerializeField] float dischargeDelayLimit;
    public GameplayManager[] satellites;
    [HideInInspector]
    public List<GameObject> activeSatellites;
    #endregion
    #region "UFO"
    [Header("UFO")]
    [SerializeField] GameObject UFOPrefab;
    [SerializeField] float attackDelayDecrease;
    [SerializeField] float delayLimit;
    [SerializeField] float instChanceIncrementUFO;
    #endregion
    #region "Meteor"
    [Header("Meteor")]
    [SerializeField] GameObject MeteorPrefab;
    [SerializeField] int damageIncrease;
    [SerializeField] int damageLimit;
    [SerializeField] float instChanceIncrementMeteor;
    #endregion
    #region "Spawners"
    [Header("Spawners")]
    [SerializeField] float ufoSpawnDelayDecrease;
    [SerializeField] float ufoSpawnDelayLimit;
    [SerializeField] float meteorSpawnDelayDecrease;
    [SerializeField] float meteorSpawnDelayLimit;
    [SerializeField] int spawnChanceIncrease;
    [SerializeField] EnemySpawnManager[] ufoSpawners;
    [SerializeField] EnemySpawnManager[] meteorSpawners;
    #endregion
    #region "Bonuses"
    [Header("Bonuses")]
    [SerializeField] GameObject ShieldBonus;
    [SerializeField] int shieldChanceDecrease;
    [SerializeField] GameObject DestroyUfoBonus;
    [SerializeField] int ufoChanceDecrease;
    [SerializeField] GameObject ChargeBonus;
    [SerializeField] int chargeChanceDecrease;
    #endregion

    private UFOBehavior _UFO;
    private MeteorBehavior _Meteor;
    private DischargeShieldBonus _Shield;
    private ChargeSatellitesBonus _Charge;
    private DestroyUfosBonus _DestroyUfo;

    private void Awake()
    {
        _UFO = UFOPrefab.GetComponent<UFOBehavior>();
        _Meteor = MeteorPrefab.GetComponent<MeteorBehavior>();
        _Shield = ShieldBonus.GetComponent<DischargeShieldBonus>();
        _DestroyUfo = DestroyUfoBonus.GetComponent<DestroyUfosBonus>();
        _Charge = ChargeBonus.GetComponent<ChargeSatellitesBonus>();

        activeSatellites = new List<GameObject>();
        for (int i=0; i<satellites.Length; i++)
        {
            activeSatellites.Add(satellites[i].gameObject);
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(IncreaseDifficulty), firstDiffIncreaseDelay, diffIncreaseDelay);
    }
    public void IncreaseDifficulty()
    {
        DecreaseDischargeDelay();
        IncreaseEnergyDecrement();
        IncreaseUFOAttackFrequency();
        DecreaseUfoSpawnDelay();
        DecreaseMeteorSpawnDelay();
        DecreaseBonusesInstantiateChance();
    }
    private void DecreaseDischargeDelay()
    {
        foreach (var satel in satellites)
        {
            if (satel.decrementDelay >= dischargeDelayLimit)
            {
                satel.decrementDelay -= dischargeDelay;
            }
        }
    }
    private void IncreaseEnergyDecrement()
    {
        foreach (var satel in satellites)
        {
            satel.energyDecrement += energyUsageIncrement;
        }
    }
    private void IncreaseUFOAttackFrequency()
    {
        if (_UFO.attackDelay > delayLimit)
        {
            _UFO.attackDelay -= attackDelayDecrease;
        }
    }
    private void DecreaseUfoSpawnDelay()
    {
        foreach (var spawner in ufoSpawners)
        {
            if (spawner.spawnDelay >= ufoSpawnDelayLimit)
            {
                spawner.spawnDelay -= ufoSpawnDelayDecrease;
                IncreaseUfoSpawnChance();
            }
        }
    }
    private void DecreaseMeteorSpawnDelay()
    {
        foreach (var spawner in meteorSpawners)
        {
            if (spawner.spawnDelay >= meteorSpawnDelayLimit)
            {
                spawner.spawnDelay -= meteorSpawnDelayDecrease;
                IncreaseMeteorSpawnChance();
            }
        }
    }
    private void IncreaseUfoSpawnChance()
    {
        foreach (var ufoSpawner in ufoSpawners)
        {
            ufoSpawner.chanceToInstantiate = Mathf.Clamp01(ufoSpawner.chanceToInstantiate + instChanceIncrementUFO);
        }
    }
    private void IncreaseMeteorSpawnChance()
    {
        foreach (var meteorSpawner in ufoSpawners)
        {
            meteorSpawner.chanceToInstantiate = Mathf.Clamp01(meteorSpawner.chanceToInstantiate + instChanceIncrementMeteor);
        }
    }
    private void DecreaseBonusesInstantiateChance()
    {
        DecreaseChanceOfBonus(_Charge, chargeChanceDecrease);
        DecreaseChanceOfBonus(_Shield, shieldChanceDecrease);
        DecreaseChanceOfBonus(_DestroyUfo, ufoChanceDecrease);
    }
    private void DecreaseChanceOfBonus(BonusManager bonus, float chanceDecrease)
    {
        bonus.chanceToInstantiate = Mathf.Clamp01(bonus.chanceToInstantiate + chanceDecrease);
    }
}