using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Swipe control")]
    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;

    [SerializeField] private float swipeRange;
    [SerializeField] private float tapRange;

    [Header("Camera")]
    [SerializeField, Range(0f, 1f)] float smoothFactor;
    [SerializeField] Transform targetToFollow;
    [SerializeField] Transform rotateAroundJoint;
    [SerializeField] float angleChange;

    private float _posX;
    private float _posZ;

    private void FixedUpdate()
    {
        SwipeHorizontal();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _posX = Mathf.Lerp(transform.position.x, targetToFollow.position.x, smoothFactor);
        _posZ = Mathf.Lerp(transform.position.z, targetToFollow.position.z, smoothFactor);
        transform.position = new Vector3(_posX, transform.position.y, _posZ);
    }

    public void TurnLeft()
    {
        targetToFollow.RotateAround(rotateAroundJoint.position, Vector3.up, angleChange);
        transform.Rotate(angleChange * Vector3.up);
    }

    public void TurnRight()
    {
        targetToFollow.RotateAround(rotateAroundJoint.position, Vector3.down, angleChange);
        transform.Rotate(angleChange * Vector3.down);
    }

    private void SwipeHorizontal()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

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
        {
            stopTouch = false;
            endTouchPosition = Input.GetTouch(0).position;
        }
    }
}
