using System.Collections;
using System.Collections.Generic;
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

    public void PlayClip(AudioClip clip)
    {
        if (randomizePitch)
        {
            RandomizePitch();
            _globalAudioSource.pitch = _randomizedPitch;
        }
        _globalAudioSource.PlayOneShot(clip);
    }

    private void RandomizePitch() => _randomizedPitch = Random.Range(0.9f, 1.1f);
}
