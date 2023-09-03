using System.Collections;
using UnityEngine;

public sealed class BG_Music : MonoBehaviour
{
    public static BG_Music Instance;

    [SerializeField, Range(0.5f, 3f)] private float fadeDuration;

    public float FadeDuration => fadeDuration;

    private AudioSource _source;

    private void Awake()
    {
        Instance = this;
        _source = GetComponent<AudioSource>();
    }

    public void StartPlay()
    {
        FadeIn();
        _source.Play();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    private IEnumerator FadeOutCoroutine()
    {
        float startVolume = _source.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            _source.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        _source.Stop();
        _source.volume = startVolume;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }
    private IEnumerator FadeInCoroutine()
    {
        float startVolume = 0f;
        float timer = 0f;

        _source.volume = 0f;
        _source.Play();

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            _source.volume = Mathf.Lerp(startVolume, 1f, timer / fadeDuration);
            yield return null;
        }

        _source.volume = 1f;
    }
}
