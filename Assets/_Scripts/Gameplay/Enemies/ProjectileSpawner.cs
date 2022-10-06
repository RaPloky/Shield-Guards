using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField, Range(0.1f, 10f)] private float launchDelay;
    [SerializeField, Range(0f, 1f)] private float launchChance;

    private Transform _target;

    public Transform Target => _target;

    private void Start()
    {
        StartCoroutine(LaunchProjectile());
        FindClosestTarget();
    }

    private IEnumerator LaunchProjectile()
    {
        while (true)
        {
            yield return new WaitForSeconds(launchDelay);

            if (IsLaunchAllowed())
                Instantiate(projectilePrefab, gameObject.transform);
        }
    }

    private bool IsLaunchAllowed()
    {
        float randomChance = Random.Range(0f, 1f);
        return randomChance < launchChance;
    }

    private void FindClosestTarget()
    {
        int closestTargetIndex = 0;
        float distanceToClosestTarget = float.MaxValue;
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Satellite");

        if (targets == null)
            return;

        for (int i = 0; i < targets.Length; i++)
        {
            Transform targetTrans = targets[i].transform;
            float distanceBetweenSpawnerAndTarget = Vector3.Distance(transform.position, targetTrans.position);

            if (distanceBetweenSpawnerAndTarget < distanceToClosestTarget)
            {
                distanceToClosestTarget = distanceBetweenSpawnerAndTarget;
                closestTargetIndex = i;
            }
        }
        _target = targets[closestTargetIndex].transform;
    }
}
