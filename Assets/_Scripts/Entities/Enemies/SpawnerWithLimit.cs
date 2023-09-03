using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithLimit : Spawner
{
    [SerializeField] private List<Spawner> otherCarrierSpawners;

    // prevention from spamming "Carrier":
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

                NotifyAboutDanger();
                prefabToOperate.SetActive(true);
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
