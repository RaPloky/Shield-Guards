using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllEnemies : Bonus
{
    [SerializeField] private List<Spawner> spawners;
    private float spawnFreezeTime;

    private GameObject[] _enemies;

    private void Start()
    {
        spawnFreezeTime = UpgradeManager.Instance.CurrDemolitionEffectValue;
    }

    public void DestroyEnemies()
    {
        if (!isBonusEnabled)
            return;

        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in _enemies)
            StartCoroutine(enemy.GetComponent<Enemy>().DestroyThatEnemy());

        StartCoroutine(FreezeSpawn());

        effectDurationGO.SetActive(true);
        StartEffectTimer(spawnFreezeTime);
        isBonusEnabled = false;
    }

    private IEnumerator FreezeSpawn()
    {
        foreach (Spawner spawner in spawners)
            spawner.IsSpawnFreezed = true;

        yield return new WaitForSeconds(spawnFreezeTime);

        foreach (Spawner spawner in spawners)
            spawner.IsSpawnFreezed = false;
    }
}
