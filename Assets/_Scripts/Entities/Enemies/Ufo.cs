using System.Collections;
using UnityEngine;

public class Ufo : Enemy
{
    [SerializeField] private ParticleSystem onDestroyParticles;
    [SerializeField] private Spawner weapon;
    [SerializeField] private GameObject shield;
    [SerializeField] private AudioClip onDamageTakenSound;

    private Transform _thatTrans;
    private Animator _animator;

    private void Start()
    {
        _difficultyManager = DifficultyUpdate.Instance;
        _startHealth = Health;

        _thatTrans = transform;
        _startPosition = relatedSpawner.transform.position;

        SetGlitchController();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        UpdateHealthBar();
        PlayOneShotSound(enableSound, ownAudioSource);
    }

    public override IEnumerator DisableThatEnemy()
    {
        yield return new WaitForSeconds(0f);

        PlayDisableSound();
        PlayParticlesOnDisable();
        PlayParticlesOnLaunchedProjectileDisable();

        DisableUfo();
        DisableDangerNotifications();

        EventManager.SendOnEnemyDisabled();
        EventManager.SendOnScoreUpdated(destructionReward);
    }

    private void DisableUfo()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = _startPosition;
        Health = _startHealth;
    }

    protected override void DamageEnemy()
    {
        base.DamageEnemy();
        ownAudioSource.PlayOneShot(onDamageTakenSound);
    }

    private void PlayParticlesOnDisable()
    {
        GameObject particles = Instantiate(onDestroyParticles.gameObject, _thatTrans.position, _thatTrans.rotation);
        particles.isStatic = true;
        onDestroyParticles.Play();
        Destroy(particles, onDestroyParticles.main.duration);
    }

    private void PlayParticlesOnLaunchedProjectileDisable()
    {
        weapon.ActiveEnemy.GetComponent<ProjectileBehavior>().PlayParticlesOnDisable();
    }

    public void PlayDissaperAnim() => _animator.SetTrigger("Dissapear");
    public float AnimLength => _animator.GetCurrentAnimatorClipInfo(0).Length;

    public void EnableCarrierShield(bool toEnable)
    {
        if (toEnable)
            shield.SetActive(true);
        else
            shield.SetActive(false);
    }
}
