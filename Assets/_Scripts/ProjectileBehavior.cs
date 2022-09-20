using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(0f, 0.05f)] float speedFactor;
    [SerializeField] int energyDamage;

    private Transform _target;
    private Rigidbody _thatRB;

    private void Awake()
    {
        _target = GetTargetFromParent();
        _thatRB = GetComponent<Rigidbody>();
        transform.SetParent(null);
    }

    private Transform GetTargetFromParent() => transform.parent.GetComponent<ProjectileSpawner>().Target;

    private void FixedUpdate()
    {
        float targetX = _target.position.x - transform.position.x;
        float targetY = _target.position.y - transform.position.y;
        float targetZ = _target.position.z - transform.position.z;

        _thatRB.AddForce(targetX, targetY, targetZ, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Stats target = collision.gameObject.GetComponent<Stats>();

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        target.ConsumptEnergy(energyDamage);
        Destroy(gameObject);
    }
}
