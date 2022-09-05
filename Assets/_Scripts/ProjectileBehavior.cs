using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(0f, 0.05f)] float speedFactor;
    [SerializeField] int energyDamage;

    private Transform _target;

    private void Awake()
    {
        _target = GetTargetFromParent();
    }

    private Transform GetTargetFromParent() => transform.parent.GetComponent<ProjectileSpawner>().Target;

    private void FixedUpdate()
    {
        float posX = Mathf.Lerp(transform.position.x, _target.position.x, speedFactor);
        float posY = Mathf.Lerp(transform.position.y, _target.position.y, speedFactor);
        float posZ = Mathf.Lerp(transform.position.z, _target.position.z, speedFactor);

        transform.position = new Vector3(posX, posY, posZ);
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
