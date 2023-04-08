using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(0f, 0.05f)] private float speedFactor;
    [SerializeField] private int energyDamage;
    [SerializeField] private bool isMeteor;
    [SerializeField, Range(0f, 1f)] private float glitchStrength;
    [SerializeField] private ParticleSystem onDestroyParticles;

    private Transform _targetTrans;
    private Rigidbody _thatRb;
    private Transform _thatTrans;

    private float _targetX;
    private float _targetY;
    private float _targetZ;

    private Guard _targetComponent;
    private Meteor _thatMeteorReference;

    private void Awake()
    {
        _thatTrans = transform;
        _targetTrans = GetTargetFromParent();
        _thatRb = GetComponent<Rigidbody>();

        if (isMeteor)
            _thatMeteorReference = GetComponent<Meteor>();
    }

    private Transform GetTargetFromParent() => _thatTrans.parent.GetComponent<Spawner>().Target;

    private void FixedUpdate()
    {
        _targetX = _targetTrans.position.x - _thatTrans.position.x;
        _targetY = _targetTrans.position.y - _thatTrans.position.y;
        _targetZ = _targetTrans.position.z - _thatTrans.position.z;

        _thatRb.AddForce(_targetX, _targetY, _targetZ, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _targetComponent = collision.gameObject.GetComponent<Guard>();

        if (_targetComponent == null)
        {
            DestoyThatProjectile();
            return;
        }
        _targetComponent.ConsumptEnergy(energyDamage);
        DestoyThatProjectile();
        PlayHitGlitchAnim();
    }

    private void DestoyThatProjectile()
    {
        PlayParticlesOnDestroy();

        if (isMeteor)
            StartCoroutine(_thatMeteorReference.DestroyThatEnemy());
        else
            Destroy(gameObject);
    }

    public void PlayParticlesOnDestroy()
    {
        GameObject particles = Instantiate(onDestroyParticles.gameObject, _thatTrans.position, _thatTrans.rotation);
        onDestroyParticles.Play();
        Destroy(particles, onDestroyParticles.duration);
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
