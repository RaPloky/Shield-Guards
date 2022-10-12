using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllEnemies : MonoBehaviour
{
    [SerializeField] private bool isBonusEnabled;

    private GameObject[] _enemies;

    public bool IsBonusEnabled
    {
        get => isBonusEnabled;
        set => isBonusEnabled = value;
    }

    public void DestroyEnemies()
    {
        if (!isBonusEnabled)
            return;

        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in _enemies)
            StartCoroutine(enemy.GetComponent<Enemy>().DestroyThatEnemy());

        isBonusEnabled = false;
    }
}
