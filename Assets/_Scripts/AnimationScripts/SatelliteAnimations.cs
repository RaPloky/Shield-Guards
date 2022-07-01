using UnityEngine;

public class SatelliteAnimations : MonoBehaviour
{
    [SerializeField] Animator bonusButtContr;
    [SerializeField] Animator bodyContr;
    [SerializeField] string bodyAnimName;

    private void Start()
    {
        StartIdleAnimation();
    }
    private void StartIdleAnimation()
    {
        GetComponent<Animator>().Play(bodyAnimName);
    }
    private void OnMouseDown()
    {
        bodyContr.Play(bodyAnimName, -1, 0f);
        bonusButtContr.Play("ChangeSize", -1, 0f);
    }
}
