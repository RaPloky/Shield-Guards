using UnityEngine;

public class RotateAroundSelf : MonoBehaviour
{
    [SerializeField, Range(-0.5f, 0.5f)] private float localRotationSpeed;

    private Transform _trans;

    private void Awake() => _trans = transform;

    private void FixedUpdate() => _trans.Rotate(0, localRotationSpeed * Time.deltaTime, 0, Space.Self);
}
