using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotateSpeed = 1f;
    public Material skyboxMaterial;

    void Update()
    {
        // Rotate the skybox material around the Y-axis
        skyboxMaterial.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
