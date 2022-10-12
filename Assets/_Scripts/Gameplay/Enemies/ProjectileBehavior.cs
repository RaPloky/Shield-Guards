using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(0f, 0.05f)] float speedFactor;
    [SerializeField] int energyDamage;

    private Transform _targetRb;
    private Rigidbody _thatRB;

    private float _targetX;
    private float _targetY;
    private float _targetZ;

    private Stats _targetComponent;

    private void Awake()
    {
        _targetRb = GetTargetFromParent();
        _thatRB = GetComponent<Rigidbody>();
        transform.SetParent(null);
    }

    private Transform GetTargetFromParent() => transform.parent.GetComponent<Spawner>().Target;

    private void FixedUpdate()
    {
        _targetX = _targetRb.position.x - transform.position.x;
        _targetY = _targetRb.position.y - transform.position.y;
        _targetZ = _targetRb.position.z - transform.position.z;

        _thatRB.AddForce(_targetX, _targetY, _targetZ, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _targetComponent = collision.gameObject.GetComponent<Stats>();

        if (_targetComponent == null)
        {
            Destroy(gameObject);
            return;
        }

        _targetComponent.ConsumptEnergy(energyDamage);
        Destroy(gameObject);
    }
}
