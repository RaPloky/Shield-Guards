using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action OnEnergyValueChanged;

    public static void SendOnEnergyValueChanged()
    {
        OnEnergyValueChanged?.Invoke();
    }
}
