using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinoDigital;
using KinoAnalog;

public class GlitchAnimationController : MonoBehaviour
{
    public static GlitchAnimationController Instance;

    [SerializeField] private DigitalGlitch digitalGlitch;
    [SerializeField] private AnalogGlitch analogGlitch;

    [SerializeField, Range(0.01f, 0.1f)] private float delay;
    [SerializeField, Range(0.01f, 0.1f)] private float intensityChange;

    private float _tempColorDrift, _tempHorizontalShake, _tempDigitalIntensity, _tempScanJitter, _tempVerticalJump;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnableNoise();
    }

    public void FadeIn()
    {
        StartCoroutine(DegradeDigitalIntensity());
        StartCoroutine(DegradeAnalogColorDrift(0.5f));
    }

    private IEnumerator DegradeDigitalIntensity()
    {
        digitalGlitch.intensity = 0.6f;
        while (digitalGlitch.intensity > 0)
        {
            yield return new WaitForSeconds(delay);
            digitalGlitch.intensity = Mathf.Clamp(digitalGlitch.intensity - intensityChange, 0, 1);
        }
    }

    private IEnumerator DegradeAnalogColorDrift(float driftStrength)
    {
        analogGlitch.colorDrift = driftStrength;
        while (analogGlitch.colorDrift > 0)
        {
            yield return new WaitForSeconds(delay);
            analogGlitch.colorDrift = Mathf.Clamp(analogGlitch.colorDrift - intensityChange, 0, 1);
        }
    }

    private IEnumerator Scan(float scanStrength)
    {
        analogGlitch.scanLineJitter = scanStrength;
        while (analogGlitch.scanLineJitter > 0)
        {
            yield return new WaitForSeconds(delay);
            analogGlitch.scanLineJitter = Mathf.Clamp(analogGlitch.scanLineJitter - intensityChange, 0, 1);
        }
        analogGlitch.scanLineJitter = _tempScanJitter;
    }

    public void EnableHorizontalShake()
    {
        AssignTempColorDrift();
        AssignTempHorizontalShake();

        analogGlitch.colorDrift = 0.3f;
        analogGlitch.horizontalShake = 0.035f;
    }

    public void DisableHorizontalShake()
    {
        analogGlitch.colorDrift = _tempColorDrift;
        analogGlitch.horizontalShake = _tempHorizontalShake;
    }

    public void EnableNoise()
    {
        AssignTempScanJitter();
        AssignTempVerticalJump();

        analogGlitch.scanLineJitter = 0.1f;
        analogGlitch.verticalJump = 0.01f;
    }

    public void DisableNoise()
    {
        analogGlitch.scanLineJitter = _tempScanJitter;
        analogGlitch.verticalJump = _tempVerticalJump;
    }

    public void PlayStrongScan() => StartCoroutine(Scan(0.85f));
    public void PlayWeakScan()
    {
        StartCoroutine(DegradeAnalogColorDrift(0.3f));
        StartCoroutine(Scan(0.4f));
    }

    private void AssignTempHorizontalShake() => _tempHorizontalShake = analogGlitch.horizontalShake;
    private void AssignTempColorDrift() => _tempColorDrift = analogGlitch.colorDrift;
    private void AssignTempScanJitter() => _tempScanJitter = analogGlitch.scanLineJitter;
    private void AssignTempVerticalJump() => _tempVerticalJump = analogGlitch.verticalJump;
    private void AssignTempDigitalIntesity() => _tempDigitalIntensity = digitalGlitch.intensity;
}
