using System.Collections;
using UnityEngine;

public class StopLaserBeam : MonoBehaviour
{
    [SerializeField] LineRenderer laser;
    [SerializeField] [Range(0f, 50f)] float laserLimitZ;
    [SerializeField] Animator beamBeginning;
    [SerializeField] Animator particlesBeginning;
    [SerializeField] Animator beamEnding;
    [SerializeField] Animator particlesEnd;
    [SerializeField] [Range(0f, 2f)] float offEndBeamInSec;

    private SatelliteBehavior _satellite;
    private bool _isLaserOff = false;
    private float _descaleDelay;

    private void Awake()
    {
        _satellite = GetComponent<SatelliteBehavior>();
        _descaleDelay = Mathf.Abs(offEndBeamInSec / laser.GetPosition(0).z);
    }
    private void Update()
    {
        if (_satellite.isDicharged && !_isLaserOff)
        {
            _isLaserOff = true;
            beamBeginning.Play("OffStartBeam");
            particlesBeginning.Play("OffStartParticles");
            StartCoroutine(IDescaleLaser(_descaleDelay));
            StartCoroutine(IOffBeamInSeconds(offEndBeamInSec));
        }
    }
    private IEnumerator IOffBeamInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        beamEnding.Play("OffEndBeam");
        particlesEnd.Play("OffEndParticles");
    }

    private IEnumerator IDescaleLaser(float descaleDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(descaleDelay);
            float laserLength = laser.GetPosition(1).z + 1;
            if (laserLength == laserLimitZ + 1) yield break;

            laser.SetPosition(1, new Vector3(0f, 0f, laserLength));
        }
    } 
}
