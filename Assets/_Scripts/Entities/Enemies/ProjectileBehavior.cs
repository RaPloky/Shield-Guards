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
    private float _x, _y, _z;

    private void Awake()
    {
        _thatTrans = transform;
        _startPos = parentSpawner.position;

        if (isMeteor)
            _thatMeteorReference = GetComponent<Meteor>();
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        _x = targetTrans.position.x - _thatTrans.position.x;
        _y = targetTrans.position.y - _thatTrans.position.y;
        _z = targetTrans.position.z - _thatTrans.position.z;

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
            StartCoroutine(_thatMeteorReference.DisableThatEnemy());
        else
        {
            PlayParticlesOnDisable();

            gameObject.SetActive(false);
            gameObject.transform.position = _startPos;
        }
    }

    public void PlayParticlesOnDisable()
    {
        GameObject particles = Instantiate(onDisableParticles.gameObject, _thatTrans.position, _thatTrans.rotation);
        particles.isStatic = true;
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
