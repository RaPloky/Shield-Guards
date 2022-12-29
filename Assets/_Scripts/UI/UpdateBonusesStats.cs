using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBonusesStats : MonoBehaviour
{
    private UpgradeManager _upgradeManager;

    [SerializeField] private Image chargeStatus;
    [SerializeField] private Image demolitionStatus;
    [SerializeField] private Image protectionStatus;

    [SerializeField] private TextMeshProUGUI chargeInfo;
    [SerializeField] private TextMeshProUGUI demolitionInfo;
    [SerializeField] private TextMeshProUGUI protectionInfo;

    private void Start()
    {
        _upgradeManager = UpgradeManager.instance;
        UpdateChargeStatus();
        UpdateDemolitionStatus();
        UpdateProtectionStatus();
    }

    public void UpdateChargeStatus()
    {
        chargeStatus.fillAmount = _upgradeManager.ChargingBonusLvl / _upgradeManager.levelsLimit;
        chargeInfo.text = $"+{_upgradeManager.CurrentChargeValue} per charging";
    }

    public void UpdateDemolitionStatus()
    {
        demolitionStatus.fillAmount = _upgradeManager.DemolitionBonusLvl / _upgradeManager.levelsLimit;
        demolitionInfo.text = $"{_upgradeManager.CurrentDemolitionValue}s enemies spawn freeze after using";
    }

    public void UpdateProtectionStatus()
    {
        protectionStatus.fillAmount = _upgradeManager.ProtectionBonusLvl / _upgradeManager.levelsLimit;
        protectionInfo.text = $"{_upgradeManager.CurrentProtectionValue}s of invulnerability";
    }
}
