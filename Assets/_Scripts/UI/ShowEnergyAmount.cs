using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowEnergyAmount : MonoBehaviour
{
    [SerializeField] private Guard _trackedGuard;
    private TextMeshProUGUI _thatText;

    private void Start()
    {
        _thatText = GetComponent<TextMeshProUGUI>();
        UpdateAmount();
    }

    private void OnEnable()
    {
        EventManager.OnEnergyValueChanged += UpdateAmount;
    }

    private void OnDisable()
    {
        EventManager.OnEnergyValueChanged -= UpdateAmount;
    }

    private void UpdateAmount()
    {
        _thatText.text = $"{Mathf.RoundToInt(_trackedGuard.Energy * 100 / _trackedGuard.MaxEnergy)}%";
    }
}
