using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1.2f;
    [SerializeField] bool isRandomize;
    [SerializeField][Range(0, 2f)] float randomizeRange;

    private void Awake()
    {
        if (isRandomize)
            rotateSpeed = Random.Range(-randomizeRange, randomizeRange);
    }
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
