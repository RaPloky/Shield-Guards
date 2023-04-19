using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float spawnDelay;
    [SerializeField, Range(0f, 1f)] private float launchChance;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform target;
    [SerializeField] private Transform parent;

    [Header("Danger notifications")]
    [SerializeField] private bool isProjectileSpawner;
    [SerializeField] private List<Animator> dangerNotificators;

    private Transform _thatTrans;

    public GameObject SpawnedPrefab { get; set; }
    public Transform Target => target;
    public bool IsSpawnFreezed { get; set; }

    private Guard _targetGuard;

    public float SpawnDelay
    {
        get => spawnDelay;
        set => spawnDelay = Mathf.Clamp(value, 0.1f, 10f);
    }

    public float LaunchChance
    {
        get => launchChance;
        set => launchChance = Mathf.Clamp01(value);
    }

    private void Start()
    {
        _thatTrans = transform;
        IsSpawnFreezed = false;

        if (!isProjectileSpawner)
            _targetGuard = Target.GetComponent<Guard>();

        if (Target == null)
        {
            target = GetTargetFromSpawner();
            _targetGuard = Target.GetComponent<Guard>();
        }

        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (!_targetGuard.IsHaveEnergy)
            {
                Ufo ufo = null;
                if (parent != null)
                    ufo = parent.gameObject.GetComponent<Ufo>();

                if (ufo == null)
                {
                    DisableDanger();
                    yield break;
                }
                else
                {
                    DisableDanger();
                    ufo.PlayDissaperAnim();

                    yield return new WaitForSeconds(ufo.AnimLength);
                    ufo.ParentSpawner.enabled = false;
                    yield break;
                }
            }

            if (IsSpawnAllowed() && SpawnedPrefab == null && !IsSpawnFreezed && _targetGuard.IsHaveEnergy)
            {
                SpawnedPrefab = Instantiate(prefabToSpawn, _thatTrans);

                if (!isProjectileSpawner)
                    SpawnedPrefab.GetComponent<Enemy>().ParentSpawner = this;

                NotifyAboutDanger();
            }
        }
    }

    private void NotifyAboutDanger()
    {
        if (isProjectileSpawner)
            return;

        for (int i = 0; i < dangerNotificators.Count; i++)
            dangerNotificators[i].SetTrigger("DangerBegin");
    }

    public void DisableDanger()
    {
        if (isProjectileSpawner)
            return;

        for (int i = 0; i < dangerNotificators.Count; i++)
            dangerNotificators[i].SetTrigger("DangerOver");
    }

    private bool IsSpawnAllowed()
    {
        float randomChance = Random.Range(0f, 1f);
        return randomChance < launchChance;
    }

    // Spanwer is parent of UFO which is parent of WholeBody which is parent of gameObject:
    private Transform GetTargetFromSpawner() => _thatTrans.parent.transform.parent.transform.GetComponent<Spawner>().Target;
}
