using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllEnemies : Bonus
{
    private GameObject[] _enemies;

    public void DestroyEnemies()
    {
        if (!isBonusEnabled)
            return;

        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in _enemies)
            StartCoroutine(enemy.GetComponent<Enemy>().DestroyThatEnemy());

        isBonusEnabled = false;
        UnfillStatusIndicator();
    }
}
