using UnityEngine;

public class MicroNovaBehavior : ProjectileBehavior
{
    [Header("Sin movement")]
    [SerializeField] private float _amplitude = 1.0f; // Controls the height of the sine wave
    [SerializeField] private float _frequency = 2.0f; // Controls the number of cycles per second

    private Vector3 _direction;
    private float _timeOffset;
    private float _yOffset;

    private void Awake()
    {
        SetData();
    }

    protected override void MoveToTarget()
    {
        _direction = (targetTrans.position - transform.position).normalized;

        _timeOffset = Time.time * _frequency;
        _yOffset = Mathf.Sin(_timeOffset) * _amplitude;

        transform.position += _direction * speedFactor * Time.deltaTime + new Vector3(_yOffset, 0, 0);
    }
}
