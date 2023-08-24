using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransitionSounds : MonoBehaviour
{
    [SerializeField] private AudioClip sound1;
    [SerializeField] private AudioClip sound2;
    [SerializeField] private AudioClip sound3;
    [SerializeField] private AudioClip sound4;
    [SerializeField] private AudioClip sound5;

    private AudioSource _thatSource;

    private void Awake()
    {
        _thatSource = GetComponent<AudioSource>();
    }

    public void PlaySound1() => _thatSource.PlayOneShot(sound1);
    public void PlaySound2() => _thatSource.PlayOneShot(sound2);
    public void PlaySound3() => _thatSource.PlayOneShot(sound3);
    public void PlaySound4() => _thatSource.PlayOneShot(sound4);
    public void PlaySound5() => _thatSource.PlayOneShot(sound5);
}
