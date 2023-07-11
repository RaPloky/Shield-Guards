using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Enemy
{
    [SerializeField] private ParticleSystem onDestroyParticles;
    [SerializeField] private List<Spawner> ufoSpawners;
    [SerializeField] private Transform moveAround;
    [SerializeField] private float rotationSpeed;

    private Transform _thatTrans;
    private Animator _animator;

    private void Start()
    {
        _difficultyManager = DifficultyUpdate.Instance;
        _startHealth = Health;

        _thatTrans = transform;
        //_startPosition = relatedSpawner.transform.position;

        //_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
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

        DisableCarrier();
        DisableDangerNotifications();

        EventManager.SendOnEnemyDisabled();
        EventManager.SendOnScoreUpdated(destructionReward);

        PlayParticlesOnDisable();
    }

    private void DisableCarrier()
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
        Destroy(particles, onDestroyParticles.duration);
    }

    private void FixedUpdate()
    {
        _thatTrans.RotateAround(moveAround.position, Vector3.down, rotationSpeed * Time.deltaTime);
    }
}
