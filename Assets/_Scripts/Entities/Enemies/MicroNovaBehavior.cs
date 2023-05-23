using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroNovaBehavior : ProjectileBehavior
{
    [Header("MicroNova Properties")]
    [SerializeField] private float _zigZagAmplitude;
    [SerializeField] private float _zigZagFrequency;

    private float _timeElapsed = 0;

    private void Awake()
    {
        SetData();
    }

    protected override void MoveToTarget()
    {
        _timeElapsed += Time.deltaTime;

        float zigzagOffset = Mathf.Sin(_timeElapsed * _zigZagFrequency) * _zigZagAmplitude;
        Vector3 newPosition = _startPos + speedFactor * Time.deltaTime * (targetTrans.position - _startPos).normalized;

        newPosition += _thatTrans.right * zigzagOffset;
        _thatTrans.position = newPosition;

        // Rotate the Comet to face the target
        _thatTrans.LookAt(targetTrans);
    }
}
