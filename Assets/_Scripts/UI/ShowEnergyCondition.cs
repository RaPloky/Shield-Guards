using UnityEngine;
using UnityEngine.UI;

public class ShowEnergyCondition : MonoBehaviour
{
    [SerializeField] private Image filledImage;
    [SerializeField] private Guard trackedGuard;

    private float _newFillAmount;

    private void Start()
    {
        // Update on game start:
        UpdateFillImage();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe() => EventManager.OnEnergyValueChanged += UpdateFillImage;
    private void Unsubscribe() => EventManager.OnEnergyValueChanged -= UpdateFillImage;

    private void UpdateFillImage()
    {
        _newFillAmount = (float)trackedGuard.Energy / (float)trackedGuard.MaxEnergy;
        filledImage.fillAmount = _newFillAmount;
    }
}
