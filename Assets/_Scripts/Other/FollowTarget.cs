using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Transform _trans;

    private void Start()
    {
        _trans = transform;
    }

    private void FixedUpdate()
    {
        _trans.position = target.position;
    }
}
