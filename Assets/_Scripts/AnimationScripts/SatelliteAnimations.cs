using UnityEngine;

public class SatelliteAnimations : MonoBehaviour
{
    [SerializeField] Animator bonusButtContr;
    [SerializeField] Animator bodyContr;
    [SerializeField] string bodyAnimName;

    private SatelliteBehavior _thatSatellite;

    private void Start()
    {
        StartIdleAnimation();
        _thatSatellite = GetComponent<SatelliteBehavior>();
    }
    private void StartIdleAnimation()
    {
        GetComponent<Animator>().Play(bodyAnimName);
    }
    private void OnMouseDown()
    {
        if (_thatSatellite.isDicharged)
            return;

        bodyContr.Play(bodyAnimName, -1, 0f);
        bonusButtContr.Play("ChangeSize", -1, 0f);
    }
}
