using System.Collections.Generic;
using UnityEngine;

public class EnemyInvokeOnGuardLose : MonoBehaviour
{
    [Header("On 2 guards left")]
    [SerializeField] private List<Spawner> carrierSpawners;
    [Header("On 1 guard left")]
    [SerializeField] private List<Spawner> micronovaSpawners;

    private void ActivateSpanwers(List<Spawner> spawners)
    {
        if (spawners == null || Mathf.Approximately(spawners.Count, 0))
        {
            print($"Spawners is null");
            return;
        }

        for (int spawnerIndex = 0; spawnerIndex < spawners.Count; spawnerIndex++)
        {
            if (spawners[spawnerIndex].TargetGuard.IsHaveEnergy)
                spawners[spawnerIndex].gameObject.SetActive(true);
        }
    }

    public void ActivateCarrierSpawners() => ActivateSpanwers(carrierSpawners);
    public void ActivateMicronovaSpanwers() => ActivateSpanwers(micronovaSpawners);
}
