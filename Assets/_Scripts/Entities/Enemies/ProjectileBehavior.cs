using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(0f, 0.05f)] float speedFactor;
    [SerializeField] int energyDamage;

    private Transform _targetTrans;
    private Rigidbody _thatRb;
    private Transform _thatTrans;

    private float _targetX;
    private float _targetY;
    private float _targetZ;

    private Guard _targetComponent;

    private void Awake()
    {
        _thatTrans = transform;
        _targetTrans = GetTargetFromParent();
        _thatRb = GetComponent<Rigidbody>();
        _thatTrans.SetParent(null);
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
            Destroy(gameObject);
            return;
        }

        _targetComponent.ConsumptEnergy(energyDamage);
        Destroy(gameObject);
    }
}
