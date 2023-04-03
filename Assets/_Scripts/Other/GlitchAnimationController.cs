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

    [Range(0.01f, 0.1f)] public float shortDelay;
    [SerializeField, Range(0.01f, 0.25f)] private float intensityDelta;
    [Range(1f, 3f)] public float longDelay;

    private float _tempColorDrift, _tempHorizontalShake, _tempDigitalIntensity, _tempScanJitter, _tempVerticalJump;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnableJitterJump();
    }

    public void SingleDriftAndDigital(float digitalStartIntensity, float colorDriftStartIntensity)
    {
        StartCoroutine(DigitalFadeOut(digitalStartIntensity));
        StartCoroutine(ColorDriftFadeOut(colorDriftStartIntensity));
    }

    public void SingleDriftAndDigital()
    {
        StartCoroutine(DigitalFadeOut(0.7f));
        StartCoroutine(ColorDriftFadeOut(0.7f));
    }

    public void SingleDriftAndDigitalOut()
    {
        StartCoroutine(DigitalFadeIn());
        StartCoroutine(ColorDriftFadeIn());
    }

    public IEnumerator DigitalFadeOut(float digitalStartIntensity)
    {
        digitalGlitch.intensity = digitalStartIntensity;
        while (digitalGlitch.intensity > 0)
        {
            yield return new WaitForSeconds(shortDelay);
            digitalGlitch.intensity = Mathf.Clamp(digitalGlitch.intensity - intensityDelta, 0, 1);
        }
    }

    private IEnumerator DigitalFadeIn()
    {
        while (digitalGlitch.intensity <= 0.8f)
        {
            yield return new WaitForSeconds(longDelay);
            digitalGlitch.intensity = Mathf.Clamp(digitalGlitch.intensity + intensityDelta, 0, 1);
        }
    }

    private IEnumerator ColorDriftFadeIn()
    {
        while (analogGlitch.colorDrift <= 0.8f)
        {
            yield return new WaitForSeconds(longDelay);
            analogGlitch.colorDrift = Mathf.Clamp(analogGlitch.colorDrift + intensityDelta, 0, 1);
        }
    }

    public IEnumerator DigitalFadeInAndOut(float intensityLimit, float updateDelay)
    {
        while (digitalGlitch.intensity <= intensityLimit)
        {
            yield return new WaitForSeconds(updateDelay);
            digitalGlitch.intensity = Mathf.Clamp(digitalGlitch.intensity + intensityDelta, 0, 1);
        }

        while (digitalGlitch.intensity > 0)
        {
            yield return new WaitForSeconds(updateDelay);
            digitalGlitch.intensity = Mathf.Clamp(digitalGlitch.intensity - intensityDelta, 0, 1);
        }
    }

    private IEnumerator ColorDriftFadeOut(float driftStrength)
    {
        analogGlitch.colorDrift = driftStrength;
        while (analogGlitch.colorDrift > 0)
        {
            yield return new WaitForSeconds(shortDelay);
            analogGlitch.colorDrift = Mathf.Clamp(analogGlitch.colorDrift - intensityDelta, 0, 1);
        }
    }

    private IEnumerator Scan(float scanStrength)
    {
        analogGlitch.scanLineJitter = scanStrength;
        while (analogGlitch.scanLineJitter > 0)
        {
            yield return new WaitForSeconds(shortDelay);
            analogGlitch.scanLineJitter = Mathf.Clamp(analogGlitch.scanLineJitter - intensityDelta, 0, 1);
        }
        analogGlitch.scanLineJitter = _tempScanJitter;
    }

    public void ConstantColorShake()
    {
        AssignTempColorDrift();
        AssignTempHorizontalShake();

        analogGlitch.colorDrift = 0.3f;
        analogGlitch.horizontalShake = 0.035f;
    }

    public void DisableConstantHorizontalShake()
    {
        analogGlitch.colorDrift = _tempColorDrift;
        analogGlitch.horizontalShake = _tempHorizontalShake;
    }

    public void EnableJitterJump()
    {
        AssignTempScanJitter();
        AssignTempVerticalJump();

        analogGlitch.scanLineJitter = 0.1f;
        analogGlitch.verticalJump = 0.01f;
    }

    public void DisableJitterJump()
    {
        analogGlitch.scanLineJitter = _tempScanJitter;
        analogGlitch.verticalJump = _tempVerticalJump;
    }

    public void PlayStrongScan() => StartCoroutine(Scan(0.85f));
    public void PlayWeakScan()
    {
        StartCoroutine(ColorDriftFadeOut(0.3f));
        StartCoroutine(Scan(0.4f));
    }

    private void AssignTempHorizontalShake() => _tempHorizontalShake = analogGlitch.horizontalShake;
    private void AssignTempColorDrift() => _tempColorDrift = analogGlitch.colorDrift;
    private void AssignTempScanJitter() => _tempScanJitter = analogGlitch.scanLineJitter;
    private void AssignTempVerticalJump() => _tempVerticalJump = analogGlitch.verticalJump;
    private void AssignTempDigitalIntesity() => _tempDigitalIntensity = digitalGlitch.intensity;
}
