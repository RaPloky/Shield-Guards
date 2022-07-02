using UnityEngine;

public class SatelliteAnimations : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Animator bonusButtContr;
    [Header("Body")]
    [SerializeField] Animator bodyContr;
    [SerializeField] string bodyAnimName;
    [Header("Fire")]
    [SerializeField] Animator fireController;
    [SerializeField] string fireAnimName;

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
        fireController.Play(fireAnimName, -1, 0f);
    }
}
