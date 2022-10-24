using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(0f, 0.05f)] private float speedFactor;
    [SerializeField] private int energyDamage;
    [SerializeField] private bool isMeteor;

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
    }

    private void DestoyThatProjectile()
    {
        if (isMeteor)
            StartCoroutine(_thatMeteorReference.DestroyThatEnemy());
        else
            Destroy(gameObject);
    }
}
