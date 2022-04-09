using System.Collections;
using UnityEngine;

public class UFOBehaviour : EnemyCommonValues
{
    private DestroyUfosBonus _destroyUfosBonus;
    private Animation _hitAnimation;

    public float attackDelay = 1;
    public int enemyHealth;
    public int damageToEnemy;

    private void Awake()
    {
        SetOnAwake();
        spawnManagerTrans = gameObject.transform.parent.gameObject.GetComponent<Transform>();
        gameObject.transform.position = spawnManagerTrans.position;
        _destroyUfosBonus = GameObject.FindGameObjectWithTag("DestroyUfosBonus").GetComponent<DestroyUfosBonus>();
        _hitAnimation = GetComponent<Animation>();
    }
    private void Start()
    {
        StartCoroutine(DamagePlayer());
    }
    private void OnMouseDown()
    {
        if (PauseMenu.isGamePaused) return;
        TakeDamage();
        _hitAnimation.Play("AnimUFO_Hit");
        if (enemyHealth <= 0)
        {
            DestroyAndStartSpawn(spawnManagerTrans);
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
        gameObject.transform.position = spawnManagerTrans.position;
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
            DoDamage(satelliteToDamage);
            StartCoroutine(DamagePlayer());
        }
    }
    private void TakeDamage()
    {
        enemyHealth -= damageToEnemy;
    }
}
