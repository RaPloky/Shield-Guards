using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithLimit : Spawner
{
    [Header("Other related spawners")]
    [SerializeField] private List<Spawner> otherCarrierSpawners;

    protected override IEnumerator ActivateEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (!targetGuard.IsHaveEnergy)
            {
                Carrier childCarrier = ActiveEnemy.GetComponent<Carrier>();
                DisableDanger();
                print("Carrier Jet animation!");
                //childCarrier.PlayDissaperAnim();

                //yield return new WaitForSeconds(ufo.AnimLength);
                childCarrier.relatedSpawner.enabled = false;
                yield break;
            }

            if (IsSpawnAllowed() && !ActiveEnemy.activeSelf 
                && !IsSpawnFreezed && targetGuard.IsHaveEnergy && !IsOtherCarriersActive())
            {
                prefabToOperate.SetActive(true);
                NotifyAboutDanger();
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
