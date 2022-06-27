using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLaserBeam : MonoBehaviour
{
    [SerializeField] Animator beamBeginning;
    [SerializeField] Animator beamEnding;
    [SerializeField] float offEndBeamInSec;
    private GameplayManager _satellite;

    private void Awake()
    {
        _satellite = GetComponent<GameplayManager>();
    }

    private void Update()
    {
        if (_satellite.isDicharged)
        {
            beamBeginning.Play("BeamEnd");
            StartCoroutine(IOffBeamInSeconds(offEndBeamInSec));
            beamEnding.Play("BeamEnd");
        }
    }

    private IEnumerator IOffBeamInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
