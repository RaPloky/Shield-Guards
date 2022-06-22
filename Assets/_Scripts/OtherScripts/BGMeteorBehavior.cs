using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMeteorBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    private BGMeteorSpawner _spawner;
    private Rigidbody _rb;
    private Transform _target;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _spawner = gameObject.transform.parent.GetComponent<BGMeteorSpawner>();
        _target = _spawner.target.transform;
    }

    private void FixedUpdate()
    {
        _rb.AddForce((_target.position - _rb.position) * speed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
