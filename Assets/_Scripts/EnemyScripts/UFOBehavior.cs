﻿using System.Collections;
using UnityEngine;

public class UFOBehavior : EnemyCommonValues
{
    private DestroyUfosBonus _destroyUfosBonus;
    private Animation _hitAnim;

    public float attackDelay = 1;
    public int enemyHealth;
    public int damageToEnemy;

    private void Awake()
    {
        _spawnManagerTrans = gameObject.transform.parent.gameObject.GetComponent<Transform>();
        _hitAnim = GetComponent<Animation>();
        SetOnAwake();
        gameObject.transform.position = _spawnManagerTrans.position;
        _destroyUfosBonus = GameObject.FindGameObjectWithTag("DestroyUfosBonus").GetComponent<DestroyUfosBonus>();
    }
    private void Start()
    {
        StartCoroutine(DamagePlayer());
    }
    private void OnMouseDown()
    {
        if (PauseMenu.isGamePaused) return;
        TakeDamage();
        _hitAnim.Play();
        if (enemyHealth <= 0)
        {
            DestroyAndStartSpawn(_spawnManagerTrans);
            _destroyUfosBonus.destroyedUfosCurrCount++;
            AddScore();
            // Instantiate anti-UFO bonus:
            if (_destroyUfosBonus.destroyedUfosCurrCount >= _destroyUfosBonus.destroyedUfosCountGoal)
            {
                _destroyUfosBonus.InstantiateBonus();
            }
        }
    }
    private void FixedUpdate()
    {
        gameObject.transform.position = _spawnManagerTrans.position;
    }
    private IEnumerator DamagePlayer()
    {
        yield return new WaitForSeconds(attackDelay);

        if (_satelliteEnergy.isDicharged)
        {
            yield break;
        }
        else
        {
            DoDamage(_satelliteEnergy);
            StartCoroutine(DamagePlayer());
        }
    }
    private void TakeDamage()
    {
        enemyHealth -= damageToEnemy;
    }
}
