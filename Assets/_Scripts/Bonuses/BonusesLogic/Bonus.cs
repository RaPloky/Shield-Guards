using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

abstract public class Bonus : MonoBehaviour
{
    [SerializeField] protected bool isBonusEnabled;
    [SerializeField] protected Image bonusStatusIndicator;
    [SerializeField] protected int usageReward;
    [SerializeField] protected TextMeshProUGUI effectDurationText;
    [SerializeField] protected Button _thatButton;
    [SerializeField] protected GameObject effectDurationGO;

    public bool IsBonusEnabled
    {
        get => isBonusEnabled;
        set => isBonusEnabled = value;
    }

    public void DisableBonusButton()
    {
        _thatButton.interactable = false;
        // Play disable animation
        gameObject.SetActive(false);
    }

    public void ResetStatusIndicator() => bonusStatusIndicator.fillAmount = 0;

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
}
