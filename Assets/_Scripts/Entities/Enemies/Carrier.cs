using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Enemy
{
    [SerializeField] private ParticleSystem onDestroyParticles;
    [SerializeField] private List<Spawner> ufoSpawners;
    [SerializeField] private List<Transform> movePoints;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private float speed;

    private Transform _thatTrans;
    private Animator _animator;
    private Transform _nextMoveTarget;
    private int _movePointIndex;

    private void Start()
    {
        _movePointIndex = 0;
        _difficultyManager = DifficultyUpdate.Instance;
        _startHealth = Health;

        _thatTrans = transform;
        //_startPosition = relatedSpawner.transform.position;

        //_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        AssignNextMoveTarget();
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
        _thatTrans.LookAt(_nextMoveTarget);

        _thatTrans.position = Vector3.SmoothDamp(_thatTrans.position, _nextMoveTarget.position, ref velocity, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoveTarget"))
        {
            _movePointIndex++;
            if (Mathf.Approximately(_movePointIndex, movePoints.Count))
                _movePointIndex = 0;

            AssignNextMoveTarget();
        }
    }

    private void AssignNextMoveTarget() => _nextMoveTarget = movePoints[_movePointIndex];
}
