using System.Collections;
using UnityEngine;

public class Ufo : Enemy
{
    [SerializeField] private ParticleSystem onDestroyParticles;
    [SerializeField] private Spawner weapon;
    [SerializeField] private GameObject shield;

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
    }

    public override IEnumerator DisableThatEnemy()
    {
        yield return new WaitForSeconds(0f);

        DisableUfo();
        DisableDangerNotifications();

        EventManager.SendOnEnemyDisabled();
        EventManager.SendOnScoreUpdated(destructionReward);

        PlayParticlesOnDisable();
        PlayParticlesOnLaunchedProjectileDisable();
    }

    private void DisableUfo()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = _startPosition;
        Health = _startHealth;
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
