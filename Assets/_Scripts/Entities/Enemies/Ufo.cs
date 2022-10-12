using UnityEngine;
using System.Collections;

public class Ufo : Enemy
{
    [SerializeField] private int health;
    [SerializeField] private int damageToUfo;

    public Transform Target => _target;
    public int Health
    {
        get => health;
        set
        {
            health = (int)(Mathf.Clamp(value, 0, float.MaxValue));

            if (health <= 0)
                DestroyThatEnemy();
        }
    }

    private Transform _target;
    private Transform _thatTrans;

    private void Start()
    {
        _thatTrans = transform;
        _target = GetTargetFromSpawner();
    }

    public override IEnumerator DestroyThatEnemy()
    {
        yield return new WaitForSeconds(0);
        // Other cool code
        Destroy(gameObject);
    }
    
    private void FixedUpdate() => _thatTrans.LookAt(_target);
    private void OnMouseDown() => DamageUfo();
    private void DamageUfo() => Health -= damageToUfo;
    private Transform GetTargetFromSpawner() => _thatTrans.parent.GetComponent<Spawner>().Target;
}
