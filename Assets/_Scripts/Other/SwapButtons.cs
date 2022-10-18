using UnityEngine;

public class SwapButtons : MonoBehaviour
{
    [SerializeField] private Guard leftGuard;
    [SerializeField] private Guard centerGuard;
    [SerializeField] private Guard rightGuard;

    public bool IsLeftGuardHaveEnergy => leftGuard.IsHaveEnergy;
    public bool IsRightGuardHaveEnergy => rightGuard.IsHaveEnergy;

    private Guard _tempLeft;
    private Guard _tempCenter;
    private Guard _tempRight;

    private void Start() => SetTempGuards();

    public void OnLeftSwap()
    {
        SetTempGuards();

        leftGuard = _tempRight;
        centerGuard = _tempLeft;
        rightGuard = _tempCenter;
    }

    public void OnRightSwap()
    {
        SetTempGuards();

        leftGuard = _tempCenter;
        centerGuard = _tempRight;
        rightGuard = _tempLeft;
    }

    private void SetTempGuards()
    {
        _tempLeft = leftGuard;
        _tempCenter = centerGuard;
        _tempRight = rightGuard;
    }
}
