using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Transform _trans;

    private void Awake()
    {
        _trans = transform;
    }

    private void FixedUpdate()
    {
        _trans.LookAt(_target);
    }
}
