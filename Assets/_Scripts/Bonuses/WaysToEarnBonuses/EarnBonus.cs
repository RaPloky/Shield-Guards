using UnityEngine;
using UnityEngine.UI;

abstract public class EarnBonus : MonoBehaviour
{
    [SerializeField] protected Bonus bonus;
    [SerializeField] protected int enablingReward;
    [SerializeField] protected Image[] progressImages;

    protected int _goal;
    protected UpgradeManager _upgradeManager;
    private float _progress;

    public float Progress { get; set; }

    protected void UpdateBonusStatus()
    {
        _progress = (float)Progress / (float)_goal;

        foreach (var progressImage in progressImages)
            progressImage.fillAmount = _progress;
    }

    protected void EnableBonus()
    {
        bonus.IsBonusEnabled = true;
        bonus.ChangeActivationButtonStatus(true);
        Progress = 0;
        EventManager.SendOnScoreUpdated(enablingReward);
    }
}
