using UnityEngine;
using UnityEngine.UI;

abstract public class Bonus : MonoBehaviour
{
    [SerializeField] protected bool isBonusEnabled;
    [SerializeField] protected Image bonusStatusIndicator;

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

    public virtual void DisableBonusButton()
    {
        _thatButton.interactable = false;
        // Play disable animation
        gameObject.SetActive(false);
    }

    protected void FillStatusIndicator()
    {
        bonusStatusIndicator.fillAmount = 1;
    }

    protected void UnfillStatusIndicator()
    {
        bonusStatusIndicator.fillAmount = 0;
    }
}
