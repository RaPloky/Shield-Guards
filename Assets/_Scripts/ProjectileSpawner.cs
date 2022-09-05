using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform target;
    [SerializeField, Range(0.1f, 10f)] float launchDelay;

    public Transform Target => target;

    private void Start()
    {
        StartCoroutine(LaunchProjectile());
    }

    private IEnumerator LaunchProjectile()
    {
        while (true)
        {
            yield return new WaitForSeconds(launchDelay);
            Instantiate(projectilePrefab, gameObject.transform);
        }
    }
}
