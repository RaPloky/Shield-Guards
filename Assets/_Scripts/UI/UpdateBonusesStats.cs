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

    [Header("Levels")]
    [SerializeField] private TextMeshProUGUI chargeCurrLvl;
    [SerializeField] private TextMeshProUGUI chargeNextLvl;
    [SerializeField] private TextMeshProUGUI demolitionCurrLvl;
    [SerializeField] private TextMeshProUGUI demolitionNextLvl;
    [SerializeField] private TextMeshProUGUI protectionCurrLvl;
    [SerializeField] private TextMeshProUGUI protectionNextLvl;

    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;

        EventManager.OnBonusUpgraded += UpdateChargeStatus;
        EventManager.OnBonusUpgraded += UpdateDemolitionStatus;
        EventManager.OnBonusUpgraded += UpdateProtectionStatus;
        EventManager.OnBonusUpgraded += _upgradeManager.UpdateUpgradePrices;

        EventManager.SendOnBonusUpgraded();
    }

    private void OnDisable()
    {
        EventManager.OnBonusUpgraded -= UpdateChargeStatus;
        EventManager.OnBonusUpgraded -= UpdateDemolitionStatus;
        EventManager.OnBonusUpgraded -= UpdateProtectionStatus;
        EventManager.OnBonusUpgraded -= _upgradeManager.UpdateUpgradePrices;
    }

    public void UpdateChargeStatus()
    {
        chargeCurrLvl.text = _upgradeManager.ChargingBonusLvl + 1 < _upgradeManager.LevelsLimit ? $"lvl {_upgradeManager.ChargingBonusLvl + 1}" : "lvl max";
        chargeNextLvl.text = _upgradeManager.ChargingBonusLvl + 1 < _upgradeManager.LevelsLimit ? $"lvl {_upgradeManager.ChargingBonusLvl + 2}" : "lvl max";
        chargeInfo.text = _upgradeManager.ChargingDescription + $"\n{_upgradeManager.ChargingWayToGain}";
        chargeUpgradeInfo.text = _upgradeManager.ChargingNextLvlDesc + $"\n{_upgradeManager.ChargingNextLvlCondition}";
    }

    public void UpdateDemolitionStatus()
    {
        demolitionCurrLvl.text = _upgradeManager.DestroyingBonusLvl + 1 < _upgradeManager.LevelsLimit ? $"lvl {_upgradeManager.DestroyingBonusLvl + 1}" : "lvl max";
        demolitionNextLvl.text = _upgradeManager.DestroyingBonusLvl + 1 < _upgradeManager.LevelsLimit ? $"lvl {_upgradeManager.DestroyingBonusLvl + 2}" : "lvl max";
        demolitionInfo.text = _upgradeManager.DestroyingDescription + $"\n{_upgradeManager.DestroyingWayToGain}";
        demolitionUpgradeInfo.text = _upgradeManager.DestroyingNextLvlDesc + $"\n{_upgradeManager.DestroyingNextLvlCondition}";
    }

    public void UpdateProtectionStatus()
    {
        protectionCurrLvl.text = _upgradeManager.ProtectionBonusLvl + 1 < _upgradeManager.LevelsLimit ? $"lvl {_upgradeManager.ProtectionBonusLvl + 1}" : "lvl max";
        protectionNextLvl.text = _upgradeManager.ProtectionBonusLvl + 1 < _upgradeManager.LevelsLimit ? $"lvl {_upgradeManager.ProtectionBonusLvl + 2}" : "lvl max";
        protectionInfo.text = _upgradeManager.ProtectionDescription + $"\n{_upgradeManager.ProtectionWayToGain}";
        protectionUpgradeInfo.text = _upgradeManager.ProtectionNextLvlDesc + $"\n{_upgradeManager.ProtectionNextLvlCondition}";
    }
}
