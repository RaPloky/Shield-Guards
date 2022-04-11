using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] float diffIncreaseDelay;
    [SerializeField] float firstDiffIncreaseDelay;
    #region "Satellites"
    [Header("Satellites")]
    [SerializeField] int energyUsageIncrement;
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
    #endregion
    #region "Meteor"
    [Header("Meteor")]
    [SerializeField] GameObject MeteorPrefab;
    [SerializeField] int damageIncrease;
    [SerializeField] int damageLimit;
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
        IncreaseMeteorDamage();
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
    private void IncreaseMeteorDamage()
    {
        if (_Meteor.damageToSatellite < damageLimit)
        {
            _Meteor.damageToSatellite += damageIncrease;
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
            for (int i=0; i<ufoSpawner.spawnChance.Length; i++)
            {
                // Skip "to-instantiate" percents:
                if (ufoSpawner.spawnChance[i] == true)
                {
                    continue;
                }
                int chanceIncrement = 0;
                for (int j = 1; j <= spawnChanceIncrease; j++)
                {
                    ufoSpawner.spawnChance[i + j] = true;
                    chanceIncrement = j;
                }
                ufoSpawner.chanceToInstantiate += chanceIncrement;
                break;
            }
        }
    }
    private void IncreaseMeteorSpawnChance()
    {
        foreach (var meteorSpawner in ufoSpawners)
        {
            for (int i = 0; i < meteorSpawner.spawnChance.Length; i++)
            {
                // Skip "to-instantiate" percents:
                if (meteorSpawner.spawnChance[i] == true)
                {
                    continue;
                }
                int chanceIncrement = 0;
                for (int j = 1; j <= spawnChanceIncrease; j++)
                {
                    meteorSpawner.spawnChance[i + j] = true;
                    chanceIncrement = j;
                }
                meteorSpawner.chanceToInstantiate += chanceIncrement;
                break;
            }
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