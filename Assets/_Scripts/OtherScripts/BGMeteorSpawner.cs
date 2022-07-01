using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMeteorSpawner : MonoBehaviour
{
    [Header("MeteorTarget")]
    public GameObject target;
    [Header("MeteorSpawn")]
    [SerializeField] GameObject backgroundMeteor;
    [SerializeField] [Range(3f, 10f)] float spawnDelay;
    [SerializeField] [Range(1f, 5f)] float delayDispresion;

    private void Start()
    {
        StartCoroutine(SpawnBGMeteor());    
    }

    private IEnumerator SpawnBGMeteor()
    {
        while (true)
        {
            float delayDispersion = Random.Range(0f, delayDispresion);
            yield return new WaitForSeconds(spawnDelay + delayDispersion);
            Instantiate(backgroundMeteor, gameObject.transform);
        }
    }
}
