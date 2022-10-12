using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float spawnDelay;
    [SerializeField, Range(0f, 1f)] private float launchChance;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform target;

    public Transform Target => target;

    private void Start()
    {
        if (target == null)
            target = GetTargetFromSpawner();

        StartCoroutine(SpawnPrefab());

    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (IsSpawnAllowed())  
                Instantiate(prefabToSpawn, gameObject.transform);
        }
    }

    private bool IsSpawnAllowed()
    {
        float randomChance = Random.Range(0f, 1f);
        return randomChance < launchChance;
    }

    // Spanwer is parent of UFO which is parent of WholeBody which is parent of gameObject:
    private Transform GetTargetFromSpawner() => transform.parent.transform.parent.transform.parent.GetComponent<Spawner>().Target;
}
