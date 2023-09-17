using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer mixer;

    private const string MUSIC_PREF = "MusicVolume";
    private const string SFX_PREF = "SFXVolume";

    private const string MUSIC_PARAMETER = "Music";
    private const string SFX_PARAMETER = "SFX";

    private float MusicSavedValue => PlayerPrefs.GetFloat(MUSIC_PREF, 0);
    private float SfxSavedValue => PlayerPrefs.GetFloat(SFX_PREF, 0);

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);

        musicSlider.value = MusicSavedValue;
        sfxSlider.value = SfxSavedValue;
    }

    private void Start()
    {
        mixer.SetFloat(MUSIC_PARAMETER, Mathf.Log10(MusicSavedValue) * 20);
        mixer.SetFloat(SFX_PARAMETER, Mathf.Log10(SfxSavedValue) * 20);
    }

    public void UpdateMusicVolume(float value)
    {
        mixer.SetFloat(MUSIC_PARAMETER, Mathf.Log10(musicSlider.value) * 20);
        PlayerPrefs.SetFloat(MUSIC_PREF, musicSlider.value);
    }

    public void UpdateSFXVolume(float value)
    {
        mixer.SetFloat(SFX_PARAMETER, Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetFloat(SFX_PREF, sfxSlider.value);
    }
}
