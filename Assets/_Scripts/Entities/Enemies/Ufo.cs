using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Ufo : Enemy
{
    [SerializeField] private int health;
    [SerializeField] private int damageToUfo;
    [SerializeField] private Spawner relatedSpawner;
    [SerializeField] private List<Image> healthBarBg;
    [SerializeField] private ParticleSystem onDamageParticles;
    [SerializeField] private ParticleSystem onDestroyParticles;

    public Transform Target => _target;
    public int Health
    {
        get => health;
        set
        {
            health = (int)(Mathf.Clamp(value, 0, float.MaxValue));
            UpdateHealthBar();
            onDamageParticles.Play();

            if (health <= 0)
                StartCoroutine(DestroyThatEnemy());
        }
    }

    private Transform _target;
    private Transform _thatTrans;
    private int _startHealth;

    private void Start()
    {
        _startHealth = Health;
        _thatTrans = transform;
        _target = GetTargetFromSpawner();
        SetGlitchController();
    }

    public override IEnumerator DestroyThatEnemy()
    {
        yield return new WaitForSeconds(0);
        DisableDangerNotifications();
        Destroy(gameObject);
        PlayParticlesOnDestroy();
        PlayParticlesOnProjectileDestroy();
        
        EventManager.SendOnEnemyDestroyed();
        EventManager.SendOnScoreUpdated(destructionReward);
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused)
            return;

        DamageUfo();
    }

    private void UpdateHealthBar()
    {
        foreach (var bar in healthBarBg)
            bar.fillAmount = (float)Health / _startHealth;
    }

    private void FixedUpdate() => _thatTrans.LookAt(_target);
    private void DamageUfo() => Health -= damageToUfo;
    private Transform GetTargetFromSpawner() => _thatTrans.parent.GetComponent<Spawner>().Target;

    private void PlayParticlesOnDestroy()
    {
        GameObject particles = Instantiate(onDestroyParticles.gameObject, _thatTrans.position, _thatTrans.rotation);
        onDestroyParticles.Play();
        Destroy(particles, onDestroyParticles.duration);
    }

    private void PlayParticlesOnProjectileDestroy()
    {
        relatedSpawner.SpawnedPrefab.GetComponent<ProjectileBehavior>().PlayParticlesOnDestroy();
    }
}
