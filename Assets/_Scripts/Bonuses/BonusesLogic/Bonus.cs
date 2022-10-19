using UnityEngine;
using UnityEngine.UI;

abstract public class Bonus : MonoBehaviour
{
    [SerializeField] protected bool isBonusEnabled;
    [SerializeField] protected Image bonusStatusIndicator;
    [SerializeField] protected int usageReward;

    protected Button _thatButton;

    public bool IsBonusEnabled
    {
        get => isBonusEnabled;
        set => isBonusEnabled = value;
    }

    private void Start()
    {
        _thatButton = GetComponent<Button>();
    }

    public void DisableBonusButton()
    {
        _thatButton.interactable = false;
        // Play disable animation
        gameObject.SetActive(false);
    }

    public void ResetStatusIndicator() => bonusStatusIndicator.fillAmount = 0;

    protected void AddUsageReward() => EventManager.SendOnScoreUpdated(usageReward);
}
