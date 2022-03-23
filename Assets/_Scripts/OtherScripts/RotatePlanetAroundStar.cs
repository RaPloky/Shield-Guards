using UnityEngine;

public class RotatePlanetAroundStar : MonoBehaviour
{
    private Transform _rotateAroundObject;
    // Rotating direction:
    private Vector3 _rotationMask;

    public float rotationSpeed = 5f;

    private void Awake()
    {
        // Will rotate along Y axis:
        _rotationMask = new Vector3(0, 1, 0); 
        _rotateAroundObject = GameObject.FindGameObjectWithTag("Star").GetComponent<Transform>();
    }

    private void FixedUpdate() 
    {
        if (_rotateAroundObject)
        {
            transform.RotateAround(_rotateAroundObject.transform.position,
                _rotationMask, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(_rotationMask.x * rotationSpeed * Time.deltaTime,
                _rotationMask.y * rotationSpeed * Time.deltaTime,
                _rotationMask.z * rotationSpeed * Time.deltaTime));
        }
    }
}
