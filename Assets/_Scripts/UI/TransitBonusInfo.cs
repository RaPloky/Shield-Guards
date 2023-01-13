using UnityEngine;
using UnityEngine.UI;

public class TransitBonusInfo : MonoBehaviour
{
    [SerializeField] private UpgradeInfo bonusDecription;
    [SerializeField] private UpgradeInfo bonusUpgrade;

    private UpgradeManager _upgradeManager;

    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;
    }

    public void SetChargingDescription(Image icon)
    {
        bonusDecription.Icon.sprite = icon.sprite;
        bonusDecription.ValueInfo.text = _upgradeManager.ChargingDescription;
        bonusDecription.ConditionInfo.text = _upgradeManager.ChargingWayToGain;
    }

    public void SetDemolitonDescription(Image icon)
    {
        bonusDecription.Icon.sprite = icon.sprite;
        bonusDecription.ValueInfo.text = _upgradeManager.DemolitionDescription;
        bonusDecription.ConditionInfo.text = _upgradeManager.DemolitionWayToGain;
    }

    public void SetProtectionDescription(Image icon)
    {
        bonusDecription.Icon.sprite = icon.sprite;
        bonusDecription.ValueInfo.text = _upgradeManager.ProtectionDescription;
        bonusDecription.ConditionInfo.text = _upgradeManager.ProtectionWayToGain;
    }


    public void SetChargingUpgradeDescription(Image icon)
    {
        bonusUpgrade.Icon.sprite = icon.sprite;
        bonusUpgrade.ValueInfo.text = _upgradeManager.ChargingNextLvlDesc;
        bonusUpgrade.ConditionInfo.text = _upgradeManager.ChargingNextLvlCondition;
        bonusUpgrade.OtherInfo.text = _upgradeManager.GetNextLvlPrice(_upgradeManager.ChargingBonusLvl).ToString();
    }

    public void SetDemolitionUpgradeDescription(Image icon)
    {
        bonusUpgrade.Icon.sprite = icon.sprite;
        bonusUpgrade.ValueInfo.text = _upgradeManager.DemolitionNextLvlDesc;
        bonusUpgrade.ConditionInfo.text = _upgradeManager.DemolitionNextLvlCondition;
        bonusUpgrade.OtherInfo.text = _upgradeManager.GetNextLvlPrice(_upgradeManager.DemolitionBonusLvl).ToString();
    }

    public void SetProtectionUpgradeDescription(Image icon)
    {
        bonusUpgrade.Icon.sprite = icon.sprite;
        bonusUpgrade.ValueInfo.text = _upgradeManager.ProtectionNextLvlDesc;
        bonusUpgrade.ConditionInfo.text = _upgradeManager.DemolitionNextLvlCondition;
        bonusUpgrade.OtherInfo.text = _upgradeManager.GetNextLvlPrice(_upgradeManager.ProtectionBonusLvl).ToString();
    }
}
