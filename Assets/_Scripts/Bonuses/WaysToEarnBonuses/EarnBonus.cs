using UnityEngine;
using UnityEngine.UI;
using TMPro;

abstract public class EarnBonus : MonoBehaviour
{
    [SerializeField] protected Bonus bonus;
    [SerializeField] protected int enablingReward;
    [SerializeField] protected Image[] progressImages;
    [SerializeField] protected TextMeshProUGUI bonusAmount;

    protected int _goal;
    protected UpgradeManager _upgradeManager;
    private float _progress;

    public float Progress { get; set; }

    public void UpdateBonusStatus()
    {
        _progress = (float)Progress / (float)_goal;
        bonusAmount.text = $"{Mathf.RoundToInt(_progress * 100)}%";

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
