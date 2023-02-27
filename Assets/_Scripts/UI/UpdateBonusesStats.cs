using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBonusesStats : MonoBehaviour
{
    private UpgradeManager _upgradeManager;

    [Header("Common info description")]
    [SerializeField] private TextMeshProUGUI chargeInfo;
    [SerializeField] private TextMeshProUGUI demolitionInfo;
    [SerializeField] private TextMeshProUGUI protectionInfo;

    [Header("Upgrade time description")]
    [SerializeField] private TextMeshProUGUI chargeUpgradeInfo;
    [SerializeField] private TextMeshProUGUI demolitionUpgradeInfo;
    [SerializeField] private TextMeshProUGUI protectionUpgradeInfo;

    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;

        EventManager.OnBonusUpgraded += UpdateChargeStatus;
        EventManager.OnBonusUpgraded += UpdateDemolitionStatus;
        EventManager.OnBonusUpgraded += UpdateProtectionStatus;

        EventManager.SendOnBonusUpgraded();
    }

    private void OnDisable()
    {
        EventManager.OnBonusUpgraded -= UpdateChargeStatus;
        EventManager.OnBonusUpgraded -= UpdateDemolitionStatus;
        EventManager.OnBonusUpgraded -= UpdateProtectionStatus;
    }

    public void UpdateChargeStatus()
    {
        chargeInfo.text = _upgradeManager.ChargingDescription + $";\n{_upgradeManager.ChargingWayToGain}";
        chargeUpgradeInfo.text = _upgradeManager.ChargingNextLvlDesc + $";\n{_upgradeManager.ChargingNextLvlCondition}";
    }

    public void UpdateDemolitionStatus()
    {
        demolitionInfo.text = _upgradeManager.DemolitionDescription + $";\n{_upgradeManager.DemolitionWayToGain}";
        demolitionUpgradeInfo.text = _upgradeManager.DemolitionNextLvlDesc + $";\n{_upgradeManager.DemolitionNextLvlCondition}";
    }

    public void UpdateProtectionStatus()
    {
        protectionInfo.text = _upgradeManager.ProtectionDescription + $";\n{_upgradeManager.ProtectionWayToGain}";
        protectionUpgradeInfo.text = _upgradeManager.ProtectionNextLvlDesc + $";\n{_upgradeManager.ProtectionNextLvlCondition}";
    }
}
