using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField, Range(0f, 1f)] float smoothFactor;
    [SerializeField] Transform targetToFollow;
    [SerializeField] Transform rotateAroundJoint;
    [SerializeField] float angleChange;

    [Header("Guards relative positions")]
    [SerializeField] private Guard leftGuard;
    [SerializeField] private Guard currentGuard;
    [SerializeField] private Guard rightGuard;

    private float _posX;
    private float _posZ;
    private Transform _thatTrans;
    private Guard _tempLeft, _tempCurrent, _tempRight;

    private void Awake()
    {
        _thatTrans = transform;
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _posX = Mathf.Lerp(_thatTrans.position.x, targetToFollow.position.x, smoothFactor);
        _posZ = Mathf.Lerp(_thatTrans.position.z, targetToFollow.position.z, smoothFactor);
        _thatTrans.position = new Vector3(_posX, _thatTrans.position.y, _posZ);
    }

    public void TurnLeft()
    {
        if (!IsGuardActive(leftGuard))
        {
            return;
        }

        SwapGuardsOnSwapLeft();   
        targetToFollow.RotateAround(rotateAroundJoint.position, Vector3.up, angleChange);
        _thatTrans.Rotate(angleChange * Vector3.up);
    }

    public void TurnRight()
    {
        if (!IsGuardActive(rightGuard))
        {
            return;
        }

        SwapGuardsOnSwapRight();
        targetToFollow.RotateAround(rotateAroundJoint.position, Vector3.down, angleChange);
        _thatTrans.Rotate(angleChange * Vector3.down);
    }

    private void SwapGuardsOnSwapLeft()
    {
        AssignTempGuards();

        leftGuard = _tempRight;
        currentGuard = _tempLeft;
        rightGuard = _tempCurrent;

        ReactivateNotifications();
    }

    private void SwapGuardsOnSwapRight()
    {
        AssignTempGuards();

        leftGuard = _tempCurrent;
        currentGuard = _tempRight;
        rightGuard = _tempLeft;

        ReactivateNotifications();
    }

    private void AssignTempGuards()
    {
        _tempLeft = leftGuard;
        _tempCurrent = currentGuard;
        _tempRight = rightGuard;
    }

    public void ReactivateNotifications()
    {
        leftGuard.RelatedDangerNotifications.alpha = 0;
        rightGuard.RelatedDangerNotifications.alpha = 0;

        currentGuard.RelatedDangerNotifications.alpha = 1;
    }

    private bool IsGuardActive(Guard guard) => guard.IsHaveEnergy;
}
