using UnityEngine;

public class PlayOneShotSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool randomizePitch;

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

        _globalAudioSource.PlayOneShot(clip);
    }

    private void RandomizePitch()
    {
        _randomizedPitch = Random.Range(0.9f, 1.1f);
        _globalAudioSource.pitch = _randomizedPitch;
    }
}
