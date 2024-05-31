using System.Collections;
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

    private float _tempColorDrift, _tempScanJitter;

    private void Awake()
    {
        Instance = this;
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

    public void ConstantColorShake()
    {
        AssignTempColorDrift();

        analogGlitch.colorDrift = 0.3f;
    }

    public void DisableConstantHorizontalShake()
    {
        analogGlitch.colorDrift = _tempColorDrift;
    }

    public void DisableJitterJump()
    {
        analogGlitch.scanLineJitter = _tempScanJitter;
    }

    private void AssignTempColorDrift() => _tempColorDrift = analogGlitch.colorDrift;
    private void AssignTempScanJitter() => _tempScanJitter = analogGlitch.scanLineJitter;
}
