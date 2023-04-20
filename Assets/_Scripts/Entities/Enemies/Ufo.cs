using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Ufo : Enemy
{
    [SerializeField] private int health;
    [SerializeField] private int damageToUfo;
    [SerializeField] private List<Image> healthBarBg;
    [SerializeField] private ParticleSystem onDamageParticles;
    [SerializeField] private ParticleSystem onDestroyParticles;
    [SerializeField] private Transform target;

    private Vector3 _startPosition;

    public int Health
    {
        get => health;
        set
        {
            health = (int)(Mathf.Clamp(value, 0, float.MaxValue));
            UpdateHealthBar();
            onDamageParticles.Play();

            if (health <= 0)
                StartCoroutine(DisableThatEnemy());
        }
    }

    private Transform _thatTrans;
    private int _startHealth;
    private Animator _animator;

    private void Start()
    {
        _startHealth = Health;

        _thatTrans = transform;
        _startPosition = relatedSpawner.transform.position;

        SetGlitchController();
        _animator = GetComponent<Animator>();
    }

    public override IEnumerator DisableThatEnemy()
    {
        yield return new WaitForSeconds(0);

        DisableDangerNotifications();
        PlayParticlesOnDisable();
        PlayParticlesOnProjectileDisable();
        DisableUfo();

        EventManager.SendOnEnemyDisabled();
        EventManager.SendOnScoreUpdated(destructionReward);
    }

    private void DisableUfo()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = _startPosition;
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused)
            return;

        DamageUfo();
    }

    private void UpdateHealthBar()
    {
        for (int i = 0; i < healthBarBg.Count; i++)
            healthBarBg[i].fillAmount = (float)Health / _startHealth;
    }

    private void FixedUpdate() => _thatTrans.LookAt(target);
    private void DamageUfo() => Health -= damageToUfo;

    private void PlayParticlesOnDisable()
    {
        GameObject particles = Instantiate(onDestroyParticles.gameObject, _thatTrans.position, _thatTrans.rotation);
        particles.isStatic = true;
        onDestroyParticles.Play();
        Destroy(particles, onDestroyParticles.duration);
    }

    private void PlayParticlesOnProjectileDisable()
    {
        relatedSpawner.ActiveEnemy.GetComponent<ProjectileBehavior>().PlayParticlesOnDisable();
    }

    public void PlayDissaperAnim() => _animator.SetTrigger("Dissapear");
    public float AnimLength => _animator.GetCurrentAnimatorClipInfo(0).Length;
}
