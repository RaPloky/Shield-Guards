using UnityEngine;
using UnityEngine.UI;

abstract public class EarnBonus : MonoBehaviour
{
    [SerializeField] protected int goal;
    [SerializeField] protected Bonus bonus;
    [SerializeField] protected Image statusImage;

    public float Progress { get; set; }

    protected void UpdateBonusStatus() => statusImage.fillAmount = Progress / (float)goal;

    protected void EnableBonus()
    {
        bonus.IsBonusEnabled = true;
        Progress = 0;
    }
}
