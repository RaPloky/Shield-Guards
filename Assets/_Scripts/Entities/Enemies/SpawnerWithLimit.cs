using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithLimit : Spawner
{
    [Header("Other related spawners")]
    [SerializeField] private List<Spawner> otherCarrierSpawners;

    private bool _justSpawned = false;

    protected override IEnumerator ActivateEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (!targetGuard.IsHaveEnergy)
            {
                Carrier carrierComponent = ActiveEnemy.GetComponent<Carrier>();
                DisableDanger();
                print("Carrier apperar animation!");
                carrierComponent.relatedSpawner.enabled = false;
                yield break;
            }

            if (IsSpawnAllowed() && !ActiveEnemy.activeSelf 
                && !IsSpawnFreezed && targetGuard.IsHaveEnergy && !IsOtherCarriersActive())
            {
                if (_justSpawned)
                {
                    _justSpawned = false;
                    continue;
                }

                prefabToOperate.SetActive(true);
                NotifyAboutDanger();
                _justSpawned = true;
            }
        }
    }

    private bool IsOtherCarriersActive()
    {
        foreach (Spawner otherCarrierSpawner in otherCarrierSpawners)
        {
            if (otherCarrierSpawner.PrefabToOperate.activeSelf)
                return true;
        }
        return false;
    }
}
