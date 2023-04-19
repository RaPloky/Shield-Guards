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

    public void ActivateBonus()
    {
        if (!isBonusEnabled)
            return;

        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < _enemies.Length; i++)
            StartCoroutine(_enemies[i].GetComponent<Enemy>().DestroyThatEnemy());

        StartCoroutine(FreezeSpawn());

        effectDurationGO.SetActive(true);
        StartEffectTimer(spawnFreezeTime);
        ChangeActivationButtonStatus(false);
        isBonusEnabled = false;
    }

    private IEnumerator FreezeSpawn()
    {
        for (int i = 0; i < spawners.Count; i++)
            spawners[i].IsSpawnFreezed = true;

        yield return new WaitForSeconds(spawnFreezeTime);

        for (int i = 0; i < spawners.Count; i++)
            spawners[i].IsSpawnFreezed = false;
    }
}
