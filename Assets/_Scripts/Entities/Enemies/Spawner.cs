using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] protected float spawnDelay;
    [SerializeField, Range(0f, 1f)] protected float launchChance;
    [SerializeField] protected GameObject prefabToOperate;
    [SerializeField] private Transform parent;
    [SerializeField] protected Guard targetGuard;
    [SerializeField] private bool isDecoySpawner;

    [Header("Danger notifications")]
    [SerializeField] private bool isProjectileSpawner;
    [SerializeField] protected List<Animator> dangerNotificators;

    public Guard TargetGuard => targetGuard;
    public GameObject PrefabToOperate => prefabToOperate;
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

    protected void OnEnable()
    {
        IsSpawnFreezed = false;
        ActiveEnemy = prefabToOperate;

        if (isDecoySpawner)
            ActivateDecoyEnemySpawner();
        else
            ActivateCommonEnemySpawner();
    }

    private void ActivateDecoyEnemySpawner() => StartCoroutine(ActivateDecoyEnemy());
    protected void ActivateCommonEnemySpawner() => StartCoroutine(ActivateEnemy());

    protected virtual IEnumerator ActivateEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

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
            }
        }
    }

    private IEnumerator ActivateDecoyEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (IsSpawnAllowed())
            {
                prefabToOperate.SetActive(true);
                yield break;
            }
        }
    }

    protected void NotifyAboutDanger()
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

    protected bool IsSpawnAllowed()
    {
        float randomChance = Random.Range(0f, 1f);
        return randomChance < launchChance;
    }
}
