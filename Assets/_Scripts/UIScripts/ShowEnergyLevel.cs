using UnityEngine;
using TMPro;

public class ShowEnergyLevel : MonoBehaviour
{
    public DischargeShieldBonus dischargeShieldBonus;

    [SerializeField] GameObject satellite;
    private GameplayManager _satelliteEnergyComponent;
    private TextMeshProUGUI _energyLevelToShow;
    private int _criticalEnergyLevel;

    void Awake()
    {
        _satelliteEnergyComponent = satellite.GetComponent<GameplayManager>();
        _energyLevelToShow = gameObject.GetComponent<TextMeshProUGUI>();
        _criticalEnergyLevel = dischargeShieldBonus.criticalEnergyLevel;
    }

    void Update()
    {
        _energyLevelToShow.text = _satelliteEnergyComponent.currentEnergyLevel.ToString();

        if (_satelliteEnergyComponent.currentEnergyLevel <= _criticalEnergyLevel)
            _energyLevelToShow.color = Color.red;
        else
            _energyLevelToShow.color = Color.white;
    }
}
