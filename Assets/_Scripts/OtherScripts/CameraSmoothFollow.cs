using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject lookAt;
    [SerializeField][Range(0f,1f)] float smoothSpeed;

    private void FixedUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(lookAt.transform);
    }
}
