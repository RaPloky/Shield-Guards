using UnityEngine;
using UnityEngine.UI;

public class ShowEnergyCondition : MonoBehaviour
{
    [SerializeField] private Image[] energyImages;
    [SerializeField] private Guard trackedGuard;

    private float _newFillAmount;

    private void Start()
    {
        // Update on game start:
        UpdateEnergyCondition();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe() => EventManager.OnEnergyValueChanged += UpdateEnergyCondition;
    private void Unsubscribe() => EventManager.OnEnergyValueChanged -= UpdateEnergyCondition;

    private void UpdateEnergyCondition()
    {
        _newFillAmount = (float)trackedGuard.Energy / (float)trackedGuard.MaxEnergy;

        for (int i = 0; i < energyImages.Length; i++)
            energyImages[i].fillAmount = _newFillAmount;
    }
}
