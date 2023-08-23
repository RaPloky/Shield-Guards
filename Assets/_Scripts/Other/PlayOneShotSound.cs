using UnityEngine;

public class PlayOneShotSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool randomizePitch;
    [SerializeField] private bool isLocaledAS = false;
    [SerializeField] private AudioSource localAudioSource;

    private AudioSource _globalAudioSource;
    private float _randomizedPitch = 1;

    private void Awake()
    {
        _globalAudioSource = GameObject.FindGameObjectWithTag("GlobalSFX_Source").GetComponent<AudioSource>();
    }

    public void PlayClip()
    {
        if (randomizePitch)
            RandomizePitch();

        if (isLocaledAS)
            localAudioSource.PlayOneShot(clip);
        else
            _globalAudioSource.PlayOneShot(clip);
    }

    public void PlayClip(AudioClip customClip)
    {
        if (randomizePitch)
            RandomizePitch();

        if (isLocaledAS)
            localAudioSource.PlayOneShot(customClip);
        else
            _globalAudioSource.PlayOneShot(customClip);
    }

    private void RandomizePitch()
    {
        _randomizedPitch = Random.Range(0.9f, 1.1f);
        _globalAudioSource.pitch = _randomizedPitch;
    }
}
