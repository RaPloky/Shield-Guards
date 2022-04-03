using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    private Transform _planetTransform;
    [SerializeField] Vector3 rotateDirection;
    [SerializeField] bool randomize;
    [SerializeField] float randomizeRange;
    void Awake()
    {
        _planetTransform = GetComponent<Transform>();
        if (randomize)
        {
            rotateDirection.x = Random.Range(-randomizeRange, randomizeRange);
            rotateDirection.y = Random.Range(-randomizeRange, randomizeRange);
            rotateDirection.z = Random.Range(-randomizeRange, randomizeRange);
        }
    }
    void FixedUpdate()
    {
        _planetTransform.Rotate(rotateDirection);
    }
}
