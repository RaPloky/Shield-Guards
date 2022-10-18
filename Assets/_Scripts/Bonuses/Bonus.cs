using UnityEngine;
using UnityEngine.UI;

abstract public class Bonus : MonoBehaviour
{
    [SerializeField] protected bool isBonusEnabled;

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
}
