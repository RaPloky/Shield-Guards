﻿using System.Collections;
using UnityEngine;

public class UFOBehavior : EnemyCommonValues
{
    private DestroyUfosBonus _destroyUfosBonus;
    private Animation _hitAnim;

    public float attackDelay = 1;
    public float damageToEnemy;
    public int enemyHealth;

    private void Awake()
    {
        _spawnManagerTrans = gameObject.transform.parent.gameObject.GetComponent<Transform>();
        _hitAnim = GetComponent<Animation>();
        SetOnAwake();
        // If bonus is active:
        if (_destroyUfosBonus)
            _destroyUfosBonus = GameObject.FindGameObjectWithTag("DestroyUfosBonus").GetComponent<DestroyUfosBonus>();
    }
    private void Start()
    {
        StartCoroutine(DamagePlayer());
    }
    private void OnMouseDown()
    {
        if (PauseMenu.isGamePaused) 
            return;

        TakeDamage();
        _hitAnim.Play();
        if (enemyHealth <= 0)
        {
            AddScore();
            DestroyAndStartSpawn(_spawnManagerTrans);
            if (_destroyUfosBonus)
            {
                _destroyUfosBonus.destroyedUfosCurrCount++;
                // Instantiate anti-UFO bonus:
                if (_destroyUfosBonus.destroyedUfosCurrCount >= _destroyUfosBonus.destroyedUfosCountGoal)
                    _destroyUfosBonus.InstantiateBonus();
            }
        }
    }
    private IEnumerator DamagePlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);

            if (_satelliteEnergy.isDicharged)
                yield break;
            else
                DoDamage(_satelliteEnergy);
        }
    }
    private void TakeDamage()
    {
        enemyHealth -= (int)Mathf.Abs(damageToEnemy);
    }
}
