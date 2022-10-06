using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float smoothFactor;
    [SerializeField] Transform targetToFollow;
    [SerializeField] Transform rotateAroundJoint;
    [SerializeField] float angleChange;

    private void FixedUpdate()
    {
        float posX = Mathf.Lerp(transform.position.x, targetToFollow.position.x, smoothFactor);
        float posZ = Mathf.Lerp(transform.position.z, targetToFollow.position.z, smoothFactor);
        transform.position = new Vector3(posX, transform.position.y, posZ);
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
}
