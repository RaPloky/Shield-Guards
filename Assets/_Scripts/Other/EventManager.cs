using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action OnEnergyValueChanged;
    public static Action OnEnemyDestroyed;
    public static Action OnNonBonusEnergyAdded;

    public static void SendOnEnergyValueChanged() => OnEnergyValueChanged?.Invoke();
    public static void SendOnEnemyDestroyed() => OnEnemyDestroyed?.Invoke();
    public static void SendOnNonBonusEnergyAdded() => OnNonBonusEnergyAdded?.Invoke();
}
