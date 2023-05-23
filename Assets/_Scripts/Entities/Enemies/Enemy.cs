using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DisableThatEnemy();

    [SerializeField] protected int destructionReward;
    [SerializeField] protected int health;
    [SerializeField] protected int damageToEnemy;
    [SerializeField] protected List<Image> healthBarBg;
    [SerializeField] protected ParticleSystem onDamageParticles;

    protected GlitchAnimationController _glitchController;
    protected Vector3 _startPosition;
    protected int _startHealth;
    protected int _upgradedHealth;

    protected DifficultyUpdate _difficultyManager;

    public Spawner relatedSpawner;

    public int Health
    {
        get => health;
        set
        {
            health = (int)Mathf.Clamp(value, 0, float.MaxValue);
            UpdateHealthBar();

            if (health <= 0)
                StartCoroutine(DisableThatEnemy());
        }
    }

    protected virtual void DisableDangerNotifications()
    {
        relatedSpawner.DisableDanger();
    }

    protected virtual void UpdateHealthBar()
    {
        for (int i = 0; i < healthBarBg.Count; i++)
            healthBarBg[i].fillAmount = (float)Health / _startHealth;
    }

    protected void SetGlitchController() => _glitchController = GlitchAnimationController.Instance;

    protected void DamageEnemy()
    {
        Health -= damageToEnemy - (int)(damageToEnemy * _difficultyManager.GuardAttackDecreasePercentage);

        if (onDamageParticles != null)
            onDamageParticles.Play();
    }
}
