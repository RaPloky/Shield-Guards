using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parallax : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    private Transform _thatPart;

    private void Start()
    {
        _thatPart = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        _thatPart.Translate(_speed * Time.deltaTime, 0, 0);

        if (_thatPart.position.x >= _endPoint.position.x)
            ResetPosition();
    }

    private void ResetPosition()
    {
        _thatPart.position = _startPoint.position;
    }
}
