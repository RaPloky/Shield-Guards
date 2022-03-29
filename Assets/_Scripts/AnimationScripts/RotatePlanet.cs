using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    private Transform planetTransform;
    [SerializeField] private Vector3 rotateDirection;
    [SerializeField] private bool randomize;
    [SerializeField] private float randomizeRange;
    void Awake()
    {
        planetTransform = GetComponent<Transform>();
        if (randomize)
        {
            rotateDirection.x = Random.Range(-randomizeRange, randomizeRange);
            rotateDirection.y = Random.Range(-randomizeRange, randomizeRange);
            rotateDirection.z = Random.Range(-randomizeRange, randomizeRange);
        }
    }
    void FixedUpdate()
    {
        planetTransform.Rotate(rotateDirection);
    }
}
