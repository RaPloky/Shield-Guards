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
        chargeStatus.fillAmount = (float)_upgradeManager.ChargingBonusLvl / _upgradeManager.LevelsLimit;
        chargeInfo.text = $"+{_upgradeManager.CurrentChargeValue} per charging";
    }

    public void UpdateDemolitionStatus()
    {
        demolitionStatus.fillAmount = (float)_upgradeManager.DemolitionBonusLvl / _upgradeManager.LevelsLimit;
        demolitionInfo.text = $"{_upgradeManager.CurrentDemolitionValue}s enemies spawn freeze after using";
    }

    public void UpdateProtectionStatus()
    {
        protectionStatus.fillAmount = (float)_upgradeManager.ProtectionBonusLvl / _upgradeManager.LevelsLimit;
        protectionInfo.text = $"{_upgradeManager.CurrentProtectionValue}s of invulnerability";
    }
}
