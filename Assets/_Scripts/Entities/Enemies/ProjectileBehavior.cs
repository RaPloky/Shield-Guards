using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] protected float speedFactor;
    [SerializeField] private int energyDamage;
    [SerializeField] private bool isFragileTrash;
    [SerializeField, Range(0f, 1f)] private float glitchStrength;
    [SerializeField] private ParticleSystem onDisableParticles;
    [SerializeField] protected Transform targetTrans;
    [SerializeField] private Transform parentSpawner;

    protected Transform _thatTrans;
    protected Vector3 _startPos;

    private Guard _targetComponent;
    private Meteor _thatMeteorReference;
    private DifficultyUpdate _difficultyManager;
    private GameObject _collisionGO;

    private void Awake()
    {
        SetData();
    }

    protected void SetData()
    {
        _thatTrans = transform;
        _startPos = parentSpawner.position;
        _difficultyManager = DifficultyUpdate.Instance;

        if (isFragileTrash)
            _thatMeteorReference = GetComponent<Meteor>();
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    protected virtual void MoveToTarget()
    {
        _thatTrans.position = Vector3.MoveTowards(_thatTrans.position, targetTrans.position, speedFactor * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collisionGO = collision.gameObject;
        _targetComponent = _collisionGO.GetComponent<Guard>();

        if (_targetComponent == null)
        {
            if (_collisionGO.CompareTag("Enemy") && isFragileTrash)
            {
                Ufo ufo = _collisionGO.GetComponent<Ufo>();
                Carrier carrier = _collisionGO.GetComponent<Carrier>();

                if (ufo != null)
                    StartCoroutine(ufo.DisableThatEnemy());
                else if (carrier != null)
                    StartCoroutine(carrier.DisableThatEnemy());
            }

            DisableThatProjectile();
            return;
        }
        _targetComponent.ConsumptEnergy(energyDamage + (int)(energyDamage * _difficultyManager.EnemyDamageIncreasePercentage));
        DisableThatProjectile();
        PlayHitGlitchAnim();
    }

    private void DisableThatProjectile()
    {
        if (isFragileTrash)
        {
            StartCoroutine(_thatMeteorReference.DisableThatEnemy());
        }
        else
        {
            PlayParticlesOnDisable();

            gameObject.SetActive(false);
            gameObject.transform.SetPositionAndRotation(_startPos, Quaternion.identity);
        }
    }

    public void PlayParticlesOnDisable()
    {
        GameObject particles = Instantiate(onDisableParticles.gameObject, _thatTrans.position, _thatTrans.rotation);
        onDisableParticles.Play();
        Destroy(particles, onDisableParticles.duration);
    }

    private void PlayHitGlitchAnim()
    {
        GlitchAnimationController controller = GlitchAnimationController.Instance;
        if (isFragileTrash)
            controller.SingleDriftAndDigital(0.6f, 0.4f);
        else
            controller.PlayWeakScan();
    }
}
