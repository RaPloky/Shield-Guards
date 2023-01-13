using UnityEngine;

public enum BonusType
{
    None,
    Charging,
    Demolition,
    Protection
}

public class TransitBonusInfo : MonoBehaviour
{
    [SerializeField] private UpgradeInfo bonusDecription;
    [SerializeField] private UpgradeInfo bonusUpgrade;

    private UpgradeManager _upgradeManager;

    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;
    }
}
