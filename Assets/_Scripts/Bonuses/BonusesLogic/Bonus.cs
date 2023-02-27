using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

abstract public class Bonus : MonoBehaviour
{
    [SerializeField] protected bool isBonusEnabled;
    [SerializeField] protected int usageReward;
    [SerializeField] protected TextMeshProUGUI effectDurationText;
    [SerializeField] protected GameObject effectDurationGO;

    public bool IsBonusEnabled
    {
        get => isBonusEnabled;
        set => isBonusEnabled = value;
    }

    public void DisableBonus()
    {
        print("disabled");
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
}
