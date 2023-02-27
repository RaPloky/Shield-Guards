using UnityEngine;
using UnityEngine.UI;

abstract public class EarnBonus : MonoBehaviour
{
    [SerializeField] protected Bonus bonus;
    [SerializeField] protected int enablingReward;

    protected int _goal;
    protected UpgradeManager _upgradeManager;

    public float Progress { get; set; }

    protected void UpdateBonusStatus()
    {
        // create later
    }

    protected void EnableBonus()
    {
        bonus.IsBonusEnabled = true;
        Progress = 0;
        EventManager.SendOnScoreUpdated(enablingReward);
    }
}
