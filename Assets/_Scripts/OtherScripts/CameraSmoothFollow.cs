using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public Transform target;
    public GameObject lookAt;

    [SerializeField][Range(0f,1f)] 
    private float smoothSpeed;

    private void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed);
        transform.position = smoothedPosition;
    }
    private void FixedUpdate()
    {
        transform.LookAt(lookAt.transform);
    }
}
