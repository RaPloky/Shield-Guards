using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

abstract public class Bonus : MonoBehaviour
{
    [SerializeField] protected bool isBonusEnabled;
    [SerializeField] protected int usageReward;
    [SerializeField] protected TextMeshProUGUI effectDurationText;
    [SerializeField] protected GameObject effectDurationGO;
    [SerializeField] protected GameObject activationButton;
    [SerializeField] protected GameObject bonusChart;
    [SerializeField] protected EarnBonus bonusAmount;
    [SerializeField] protected List<ParticleSystem> activationParticleSystems;
    [SerializeField] protected GameObject bonusEnabledPS;
    [SerializeField] protected GameObject bonusProgress;
    [SerializeField] protected AudioClip activationSound;
    [SerializeField] protected AudioClip bonusReadySound;
    [SerializeField] protected AudioSource ownSource;

    protected bool _soundActivated = false;

    public bool IsBonusEnabled
    {
        get => isBonusEnabled;
        set => isBonusEnabled = value;
    }

    public void DisableBonus()
    {
        IsBonusEnabled = false;

        if (effectDurationGO != null)
            effectDurationGO.SetActive(false);
    }

    protected void AddUsageReward() => EventManager.SendOnScoreUpdated(usageReward);

    protected void StartEffectTimer(float effectDuration) => StartCoroutine(EnableTimer(effectDuration));

    protected IEnumerator EnableTimer(float effectDuration)
    {
        float effectTimeLeft = effectDuration;

        while (effectTimeLeft > 0)
        {
            yield return new WaitForFixedUpdate();
            effectTimeLeft = Mathf.Clamp(effectTimeLeft -= Time.fixedDeltaTime, 0, effectDuration);
            effectDurationText.text = effectTimeLeft.ToString("0.0");
        }
        effectDurationGO.SetActive(false);
    }

    public void ChangeActivationButtonStatus(bool status)
    {
        bonusChart.SetActive(!status);
        bonusProgress.SetActive(!status);

        bonusAmount.UpdateBonusStatus();

        activationButton.SetActive(status);
        bonusEnabledPS.SetActive(status);

        if (status && !_soundActivated)
        {
            PlaySound(bonusReadySound);
            _soundActivated = true;
        }
    }

    protected void PlayActivationParticleSystem()
    {
        for (int i = 0; i < activationParticleSystems.Count; i++)
            activationParticleSystems[i].Play();
    }

    protected void PlaySound(AudioClip clip) => ownSource.PlayOneShot(clip);
    protected void ResetSoundBool() => _soundActivated = false;
}
