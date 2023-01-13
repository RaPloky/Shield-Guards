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
        bonusUpgrade.OtherInfo.text = _upgradeManager.GetNextLvlPrice(_upgradeManager.ChargingBonusLvl + 1).ToString();
    }

    public void SetDemolitionUpgradeDescription(Image icon)
    {
        bonusUpgrade.Icon.sprite = icon.sprite;
        bonusUpgrade.ValueInfo.text = _upgradeManager.DemolitionNextLvlDesc;
        bonusUpgrade.OtherInfo.text = _upgradeManager.GetNextLvlPrice(_upgradeManager.DemolitionBonusLvl + 1).ToString();
    }

    public void SetProtectionUpgradeDescription(Image icon)
    {
        bonusUpgrade.Icon.sprite = icon.sprite;
        bonusUpgrade.ValueInfo.text = _upgradeManager.ProtectionNextLvlDesc;
        bonusUpgrade.OtherInfo.text = _upgradeManager.GetNextLvlPrice(_upgradeManager.ProtectionBonusLvl + 1).ToString();
    }
}
