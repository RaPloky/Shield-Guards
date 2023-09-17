using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DifficultyUpdate : MonoBehaviour
{
    public static DifficultyUpdate Instance;

    [SerializeField, Range(10f, 40f)] private float difficultyIncreaseDelay;

    [SerializeField] private List<Spawner> ufoSpawners;
    [SerializeField] private List<Spawner> meteorSpawners;
    [SerializeField] private List<Spawner> carrierSpawners;
    [SerializeField] private List<Spawner> micronovaSpawners;

    [SerializeField, Range(0.0f, 0.05f)] private float spawnChanceIncrease;
    [SerializeField, Range(0.0f, 0.25f)] private float spawnDelayDecrease;
    [SerializeField, Range(0, 0.25f)] private float consumptionDelayDecrease;
    [SerializeField, Range(0.2f, 0.5f)] private float consumptionDelayLimit;
    [SerializeField, Range(3f, 5f)] private float desctructMessageDelay;

    [SerializeField] private List<Guard> activeGuards;

    [Header("For charger off:")]
    [SerializeField] private List<GainEnergyOnTouch> energyUpdaters;
    [SerializeField] private GameObject chargerDestroyedMessage;

    [Header("For destroyer off:")]
    [SerializeField] private GameObject destroyerDestroyedMessage;
    [SerializeField, Range(0, 1f)] private float guardAttackDecreasePercentage;

    [Header("For protector off:")]
    [SerializeField] private GameObject protectorDestroyedMessage;
    [SerializeField, Range(0, 1f)] private float enemyDamageIncreasePercentage;

    private float _enemyDamageIncreasePercentagePropertyValue;
    private float _guardAttackDecreasePercentagePropertyValue;

    public List<Guard> ActiveGuards => activeGuards;
    public float EnemyDamageIncreasePercentage
    {
        get => _enemyDamageIncreasePercentagePropertyValue;
        set => _enemyDamageIncreasePercentagePropertyValue = Mathf.Clamp(value, 0, 1);
    }

    public float GuardAttackDecreasePercentage
    {
        get => _guardAttackDecreasePercentagePropertyValue;
        set => _guardAttackDecreasePercentagePropertyValue = Mathf.Clamp(value, 0, 1);
    }

    private WaitForSeconds DifficultyIncreaseDelay;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(IncreaseDifficulty());
        DifficultyIncreaseDelay = new WaitForSeconds(difficultyIncreaseDelay);
        _enemyDamageIncreasePercentagePropertyValue = 0;
        _guardAttackDecreasePercentagePropertyValue = 0;
    }

    private void OnEnable() => EventManager.OnGuardDischarged += IncreaseEnergyConsumption;
    private void OnDisable() => EventManager.OnGuardDischarged -= IncreaseEnergyConsumption;

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return DifficultyIncreaseDelay;

            UpdateSpawnersStats(ufoSpawners);
            UpdateSpawnersStats(meteorSpawners);
            UpdateSpawnersStats(carrierSpawners);
            UpdateSpawnersStats(micronovaSpawners);
        }
    }

    private void UpdateSpawnersStats(List<Spawner> spawners)
    {
        if (spawners == null)
            return;

        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].LaunchChance += spawnChanceIncrease;
            spawners[i].SpawnDelay -= spawnDelayDecrease;
        }
    }

    public void RemoveGuardFromList(ref Guard guard) => activeGuards.Remove(guard);

    private void IncreaseEnergyConsumption()
    {
        for (int i = 0; i < activeGuards.Count; i++)
        {
            activeGuards[i].ConsumptionDelay = Mathf.Clamp(activeGuards[i].ConsumptionDelay - consumptionDelayDecrease, 
                consumptionDelayLimit, activeGuards[i].StartConsumptionDelay);
        }
    }

    public void SpecificDifficultyIncrease(GuardType guardType)
    {
        if (guardType == GuardType.None)
        {
            print("Guard type haven't selected");
            return;
        }

        switch (guardType)
        {
            case GuardType.Charger:
                DecreaseEnergyAddAmount();
                break;
            case GuardType.Destroyer:
                IncreaseEnemyHp();
                break;
            case GuardType.Protector:
                IncreaseEnemyDamage();
                break;
        }
    }

    private void DecreaseEnergyAddAmount()
    {
        EnableMessage(chargerDestroyedMessage);
        int energyAddAmountDecrease;

        for (int i = 0; i < energyUpdaters.Count; i++)
        {
            energyAddAmountDecrease = energyUpdaters[i].EnergyAddAmount / 4;
            energyUpdaters[i].EnergyAddAmount -= energyAddAmountDecrease;
        }

        StartCoroutine(DisableMessage(chargerDestroyedMessage));
    }

    private void IncreaseEnemyHp()
    {
        EnableMessage(destroyerDestroyedMessage);

        _guardAttackDecreasePercentagePropertyValue = guardAttackDecreasePercentage;

        StartCoroutine(DisableMessage(destroyerDestroyedMessage));
    }

    private void IncreaseEnemyDamage()
    {
        EnableMessage(protectorDestroyedMessage);

        _enemyDamageIncreasePercentagePropertyValue = enemyDamageIncreasePercentage;

        StartCoroutine(DisableMessage(protectorDestroyedMessage));
    }

    private IEnumerator DisableMessage(GameObject destructionMessage)
    {
        yield return new WaitForSeconds(desctructMessageDelay);
        destructionMessage.SetActive(false);
    }

    private void EnableMessage(GameObject message)
    {
        if (Mathf.Approximately(activeGuards.Count, 0))
            return;

        message.SetActive(true);
    }

}
