using UnityEngine;

public class MicroNovaBehavior : ProjectileBehavior
{
    [Header("Sin movement")]
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

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

        _timeOffset = Time.time * frequency;
        _yOffset = Mathf.Sin(_timeOffset) * amplitude;

        transform.position += _direction * speedFactor * Time.deltaTime + new Vector3(_yOffset, 0, 0);
    }
}
