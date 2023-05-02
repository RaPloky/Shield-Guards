using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllEnemies : Bonus
{
    [SerializeField] private List<Spawner> spawners;
    [SerializeField] private Enemy[] _enemies;
    private float spawnFreezeTime;

    private void Start()
    {
        spawnFreezeTime = UpgradeManager.Instance.CurrDemolitionEffectValue;
    }

    public void ActivateBonus()
    {
        if (!isBonusEnabled)
            return;

        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i].isActiveAndEnabled)
                StartCoroutine(_enemies[i].DisableThatEnemy());
        }

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
