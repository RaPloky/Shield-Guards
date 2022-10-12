using UnityEngine;

public class Ufo : MonoBehaviour
{
    [SerializeField] private int health;

    public Transform Target => _target;

    private Transform _target;
    private Transform _thatTrans;

    private void Start()
    {
        _thatTrans = transform;
        _target = GetTargetFromSpawner();
    }

    public int Health
    {
        get => health;
        set
        {
            health = value;
        }
    }

    private void FixedUpdate()
    {
        _thatTrans.LookAt(_target);
    }

    private Transform GetTargetFromSpawner() => _thatTrans.parent.GetComponent<Spawner>().Target;
}
