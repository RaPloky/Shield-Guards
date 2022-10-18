using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Swipe control")]
    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private bool stopTouch = false;

    [SerializeField] private float swipeRange;
    [SerializeField] private float tapRange;
    [SerializeField] private SwapButtons swapButtonsManager;

    [Header("Camera")]
    [SerializeField, Range(0f, 1f)] float smoothFactor;
    [SerializeField] Transform targetToFollow;
    [SerializeField] Transform rotateAroundJoint;
    [SerializeField] float angleChange;

    private float _posX;
    private float _posZ;
    private Transform _thatTrans;

    private void Awake()
    {
        _thatTrans = transform;
    }

    private void FixedUpdate()
    {
        SwipeHorizontal();
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
        if (!IsLeftGuardActive())
        {
            // Message about discharged guard
            return;
        }
            

        targetToFollow.RotateAround(rotateAroundJoint.position, Vector3.up, angleChange);
        _thatTrans.Rotate(angleChange * Vector3.up);
        swapButtonsManager.OnLeftSwap();
    }

    public void TurnRight()
    {
        if (!IsRightGuardActive())
        {
            // Message about discharged guard
            return;
        }

        targetToFollow.RotateAround(rotateAroundJoint.position, Vector3.down, angleChange);
        _thatTrans.Rotate(angleChange * Vector3.down);
        swapButtonsManager.OnRightSwap();
    }

    private void SwipeHorizontal()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentPosition = Input.GetTouch(0).position;
            Vector2 distance = currentPosition - startTouchPosition;

            if (!stopTouch)
            {
                if (distance.x < -swipeRange)
                {
                    TurnLeft();
                    stopTouch = true;
                }
                else if (distance.x > swipeRange)
                {
                    TurnRight();
                    stopTouch = true;
                }
            }

        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            stopTouch = false;
    }

    private bool IsLeftGuardActive() => swapButtonsManager.IsLeftGuardHaveEnergy;
    private bool IsRightGuardActive() => swapButtonsManager.IsRightGuardHaveEnergy;
}
