using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private float speedFactor;
    [SerializeField] private int energyDamage;
    [SerializeField] private bool isMeteor;
    [SerializeField, Range(0f, 1f)] private float glitchStrength;
    [SerializeField] private ParticleSystem onDisableParticles;
    [SerializeField] private Transform targetTrans;
    [SerializeField] private Transform parentSpawner;

    private Transform _thatTrans;
    private Vector3 _startPos;

    private Guard _targetComponent;
    private Meteor _thatMeteorReference;
    private Spawner _parentSpawner;

    private void Awake()
    {
        _thatTrans = transform;
        _startPos = parentSpawner.position;
        _parentSpawner = parentSpawner.GetComponent<Spawner>();

        if (isMeteor)
            _thatMeteorReference = GetComponent<Meteor>();
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        _thatTrans.position = Vector3.MoveTowards(_thatTrans.position, targetTrans.position, speedFactor * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _targetComponent = collision.gameObject.GetComponent<Guard>();

        if (_targetComponent == null)
        {
            DisableThatProjectile();
            return;
        }
        _targetComponent.ConsumptEnergy(energyDamage);
        DisableThatProjectile();
        PlayHitGlitchAnim();
    }

    private void DisableThatProjectile()
    {
        PlayParticlesOnDisable();

        if (isMeteor)
        {
            StartCoroutine(_thatMeteorReference.DisableThatEnemy());
        }
        else
        {
            PlayParticlesOnDisable();

            gameObject.SetActive(false);
            gameObject.transform.position = _startPos;
            gameObject.transform.rotation = Quaternion.identity;
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
        if (isMeteor)
            controller.SingleDriftAndDigital(0.6f, 0.4f);
        else
            controller.PlayWeakScan();
    }
}
