using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBonusesStats : MonoBehaviour
{
    private UpgradeManager _upgradeManager;

    [SerializeField] private TextMeshProUGUI chargeInfo;
    [SerializeField] private TextMeshProUGUI demolitionInfo;
    [SerializeField] private TextMeshProUGUI protectionInfo;

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
        chargeInfo.text = _upgradeManager.ChargingDescription;
    }

    public void UpdateDemolitionStatus()
    {
        demolitionInfo.text = _upgradeManager.DemolitionDescription;
    }

    public void UpdateProtectionStatus()
    {
        protectionInfo.text = _upgradeManager.ProtectionDescription;
    }
}
