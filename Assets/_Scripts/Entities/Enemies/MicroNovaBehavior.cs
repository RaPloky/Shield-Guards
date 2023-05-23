using UnityEngine;

public class MicroNovaBehavior : ProjectileBehavior
{
    [Header("MicroNova Properties")]
    [SerializeField] private float _zigZagFrequency;

    private float _timeElapsed = 0;
    private float _zigZagOffset = 0;
    private float _zigZagAmplitude = 0;
    private Vector3 _direction;
    private Vector3 _updatedPosition;

    private void Awake()
    {
        SetData();
    }

    protected override void MoveToTarget()
    {
        _timeElapsed += Time.deltaTime;
        _zigZagAmplitude = Mathf.Clamp(Mathf.Sin(_timeElapsed), -0.25f, 0.25f);
        _zigZagOffset = Mathf.PingPong(_timeElapsed * _zigZagFrequency, _zigZagAmplitude);

        _direction = (targetTrans.position - _thatTrans.position).normalized;
        _updatedPosition = _thatTrans.position + speedFactor * Time.deltaTime * _direction;

        _updatedPosition += _thatTrans.right * _zigZagOffset;
        _thatTrans.position = _updatedPosition;
    }
}
