using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Rotate : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Transform _trans;

    private void Awake()
    {
        _trans = transform;
    }

    private void FixedUpdate()
    {
        _trans.Rotate(0, 0, _speed * Time.deltaTime);
    }
}
