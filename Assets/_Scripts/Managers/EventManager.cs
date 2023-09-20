using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action OnEnergyValueChanged;
    public static Action OnEnemyDestroyed;
    public static Action OnNonBonusEnergyAdded;
    public static Action<int> OnScoreUpdated;
    public static Action OnGameLose;
    public static Action OnGuardDischarged;
    public static Action OnBonusUpgraded;
    public static Action<EnemyAlarm> OnEnemyDeployed;
    public static Action<EnemyAlarm> OnEnemyReset;
    public static Action OnTutorialPopUpIndexChanged;
    public static Action OnRewardAdWatched;

    public static void SendOnEnergyValueChanged() => OnEnergyValueChanged?.Invoke();
    public static void SendOnEnemyDisabled() => OnEnemyDestroyed?.Invoke();
    public static void SendOnNonBonusEnergyAdded() => OnNonBonusEnergyAdded?.Invoke();
    public static void SendOnScoreUpdated(int addedPoints) => OnScoreUpdated?.Invoke(addedPoints);
    public static void SendOnGameLose() => OnGameLose?.Invoke();
    public static void SendOnGuardDischarged() => OnGuardDischarged?.Invoke();
    public static void SendOnBonusUpgraded() => OnBonusUpgraded?.Invoke();
    public static void SendOnEnemyDeployed(EnemyAlarm alarmType) => OnEnemyDeployed?.Invoke(alarmType);
    public static void SendOnEnemyReset(EnemyAlarm alarmType) => OnEnemyReset?.Invoke(alarmType);
    public static void SendOnTutorialPopUpIndexChanged() => OnTutorialPopUpIndexChanged?.Invoke();
    public static void SendOnRewardAdWatched() => OnRewardAdWatched?.Invoke();
}
