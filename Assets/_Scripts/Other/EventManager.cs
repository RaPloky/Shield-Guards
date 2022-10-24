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

    public static void SendOnEnergyValueChanged() => OnEnergyValueChanged?.Invoke();
    public static void SendOnEnemyDestroyed() => OnEnemyDestroyed?.Invoke();
    public static void SendOnNonBonusEnergyAdded() => OnNonBonusEnergyAdded?.Invoke();
    public static void SendOnScoreUpdated(int addedPoints) => OnScoreUpdated?.Invoke(addedPoints);
    public static void SendOnGameLose() => OnGameLose?.Invoke();
    public static void SendOnGuardDischarged() => OnGuardDischarged?.Invoke();
}
