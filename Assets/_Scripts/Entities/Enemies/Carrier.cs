using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Enemy
{
    [SerializeField] private ParticleSystem onDestroyParticles;
    [SerializeField] private List<Spawner> ufoSpawners;
    [SerializeField] private Transform moveAround;
    [SerializeField, Range(10f, 30f)] private float rotationSpeed;
    [SerializeField] private GameObject onSpawnParticles;
    [SerializeField] private AudioClip onDamageTakenSound;
    
    private Transform _thatTrans;
    private Vector3 _moveDirection;
    private Quaternion _startRotation;
    private bool _isClockwiseRotation 
    { 
        get
        {
            int direction = (int)Random.Range(0, 1);
            return Mathf.Approximately(direction, 1);
        } 
    }

    private void Awake()
    {
        _difficultyManager = DifficultyUpdate.Instance;
        _startHealth = Health;

        _thatTrans = transform;
        _startPosition = relatedSpawner.transform.position;
        _startRotation = _thatTrans.rotation;

        SetMoveDirection();
    }

    private void OnEnable()
    {
        ActivateCarrier();
        PlayOneShotSound(enableSound, ownAudioSource);
    }
    private void OnDisable()
    {
        ActivateUFOShields(false);
    }

    private void ActivateCarrier()
    {
        ResetSoundObjBool();
        PlayParticles(onSpawnParticles);
        ActivateUFOShields(true);
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

    protected override void DamageEnemy()
    {
        base.DamageEnemy();
        ownAudioSource.PlayOneShot(onDamageTakenSound);
    }
    public override IEnumerator DisableThatEnemy(bool destroyedByPlayer)
    {
        yield return new WaitForSeconds(0f);

        PlayParticles(onDestroyParticles);
        PlayDisableSound();

        DeactivateCarrier();
        DisableDangerNotifications();

        if (destroyedByPlayer)
        {
            EventManager.SendOnEnemyDisabled();
            EventManager.SendOnScoreUpdated(destructionReward);
        }
    }
    private void DeactivateCarrier()
    {
        PauseDisableSoundObjMovement();
        gameObject.SetActive(false);
        ResetStats();
    }
    private void ResetStats()
    {
        _thatTrans.SetPositionAndRotation(_startPosition, Quaternion.identity);
        Health = _startHealth;
        _thatTrans.rotation = _startRotation;
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

    private void SetMoveDirection()
    {
        if (_isClockwiseRotation)
            _moveDirection = Vector3.up;
        else
            _moveDirection = Vector3.down;
    }
}
