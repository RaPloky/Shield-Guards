using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Enemy
{
    [SerializeField] private ParticleSystem onDestroyParticles;
    [SerializeField] private List<Spawner> ufoSpawners;
    [SerializeField] private Transform moveAround;
    [SerializeField, Range(10f, 20f)] private float rotationSpeed;
    [SerializeField] private bool isClockwiseRotation;
    [SerializeField] private GameObject onSpawnParticles;
    [SerializeField] private AudioClip onDamageTakenSound;

    private Transform _thatTrans;
    private Vector3 _moveDirection;

    private void Start()
    {
        _difficultyManager = DifficultyUpdate.Instance;
        _startHealth = Health;

        _thatTrans = transform;
        _startPosition = relatedSpawner.transform.position;

        if (isClockwiseRotation)
            _moveDirection = Vector3.up;
        else
            _moveDirection = Vector3.down;
    }

    private void OnEnable()
    {
        EnableCarrier();
        PlayOneShotSound(enableSound, ownAudioSource);
    }

    private void EnableCarrier()
    {
        ResetSoundObjBool();
        PlayParticles(onSpawnParticles);
        ActivateUFOShields(true);
    }

    private void OnDisable()
    {
        ActivateUFOShields(false);
    }

    private void ActivateUFOShields(bool mode)
    {
        for (int index = 0; index < ufoSpawners.Count; index++)
        {
            if (!ufoSpawners[index].TargetGuard.IsHaveEnergy)
                continue;

            ufoSpawners[index].PrefabToOperate.GetComponent<Ufo>().EnableCarrierShield(mode);
        }
    }

    public override IEnumerator DisableThatEnemy()
    {
        yield return new WaitForSeconds(0f);

        PlayParticles(onDestroyParticles);
        PlayDisableSound();

        DisableCarrier();
        DisableDangerNotifications();

        EventManager.SendOnEnemyDisabled();
        EventManager.SendOnScoreUpdated(destructionReward);
    }

    private void DisableCarrier()
    {
        PauseDisableSoundObjMovement();
        gameObject.SetActive(false);
        _thatTrans.SetPositionAndRotation(_startPosition, Quaternion.identity);
        Health = _startHealth;
    }

    protected override void DamageEnemy()
    {
        base.DamageEnemy();
        ownAudioSource.PlayOneShot(onDamageTakenSound);
    }

    private void PlayParticles(ParticleSystem ps)
    {
        GameObject particles = Instantiate(ps.gameObject, _thatTrans.position, _thatTrans.rotation);
        particles.isStatic = true;
        ps.Play();
        Destroy(particles, ps.main.duration);
    }

    private void PlayParticles(GameObject particlesPrefab)
    {
        GameObject particles = Instantiate(particlesPrefab, _thatTrans.position, _thatTrans.rotation);
        ParticleSystem ps = particles.GetComponentInChildren<ParticleSystem>();
        particles.isStatic = true;
        ps.Play();
        Destroy(particles, ps.main.duration);
    }

    private void FixedUpdate()
    {
        _thatTrans.RotateAround(moveAround.position, _moveDirection, rotationSpeed * Time.deltaTime);
    }
}
