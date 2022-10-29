using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float spawnDelay;
    [SerializeField, Range(0f, 1f)] private float launchChance;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject enemyAppearAlarm;

    private Transform _thatTrans;

    public GameObject SpawnedPrefab { get; set; }
    public Transform Target => target;
    public GameObject AppearAlarm => enemyAppearAlarm;

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

    public bool IsSpawnFreezed { get; set; }


    private void Start()
    {
        _thatTrans = transform;
        IsSpawnFreezed = false;

        if (target == null)
            target = GetTargetFromSpawner();

        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (IsSpawnAllowed() && SpawnedPrefab == null && !IsSpawnFreezed)
                SpawnedPrefab = Instantiate(prefabToSpawn, _thatTrans);
        }
    }

    private bool IsSpawnAllowed()
    {
        float randomChance = Random.Range(0f, 1f);
        return randomChance < launchChance;
    }

    // Spanwer is parent of UFO which is parent of WholeBody which is parent of gameObject:
    private Transform GetTargetFromSpawner() => _thatTrans.parent.transform.parent.transform.parent.GetComponent<Spawner>().Target;
}
