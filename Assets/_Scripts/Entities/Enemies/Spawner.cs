using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float spawnDelay;
    [SerializeField, Range(0f, 1f)] private float launchChance;
    [SerializeField] private GameObject prefabToOperate;
    [SerializeField] private Transform parent;
    [SerializeField] private Guard targetGuard;
    [SerializeField] private bool isDecoySpawner;

    [Header("Danger notifications")]
    [SerializeField] private bool isProjectileSpawner;
    [SerializeField] private List<Animator> dangerNotificators;

    private WaitForSeconds WaitForSecondsBeforeSpawn;

    public GameObject ActiveEnemy { get; set; }
    public bool IsSpawnFreezed { get; set; }

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

    private void OnEnable()
    {
        IsSpawnFreezed = false;
        ActiveEnemy = prefabToOperate;
        WaitForSecondsBeforeSpawn = new WaitForSeconds(SpawnDelay);

        if (isDecoySpawner)
            ActivateDecoyEnemySpawner();
        else
            ActivateCommonEnemySpawner();
    }

    public void ActivateDecoyEnemySpawner() => StartCoroutine(ActivateDecoyEnemy());
    public void ActivateCommonEnemySpawner() => StartCoroutine(ActivateEnemy());

    private IEnumerator ActivateEnemy()
    {
        while (true)
        {
            yield return WaitForSecondsBeforeSpawn;

            if (!targetGuard.IsHaveEnergy)
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
                    ufo.relatedSpawner.enabled = false;
                    yield break;
                }
            }

            if (IsSpawnAllowed() && !ActiveEnemy.activeSelf && !IsSpawnFreezed && targetGuard.IsHaveEnergy)
            {
                prefabToOperate.SetActive(true);
                NotifyAboutDanger();
                yield break;
            }
        }
    }

    private IEnumerator ActivateDecoyEnemy()
    {
        while (true)
        {
            yield return WaitForSecondsBeforeSpawn;

            if (IsSpawnAllowed())
            {
                prefabToOperate.SetActive(true);
                yield break;
            }
        }
    }

    private void NotifyAboutDanger()
    {
        for (int i = 0; i < dangerNotificators.Count; i++)
        {
            dangerNotificators[i].SetBool("DangerOver", false);
            dangerNotificators[i].SetBool("DangerBegin", true);
        }
    }

    public void DisableDanger()
    {
        for (int i = 0; i < dangerNotificators.Count; i++)
        {
            dangerNotificators[i].SetBool("DangerBegin", false);
            dangerNotificators[i].SetBool("DangerOver", true);
        }
    }

    private bool IsSpawnAllowed()
    {
        float randomChance = Random.Range(0f, 1f);
        return randomChance < launchChance;
    }
}
