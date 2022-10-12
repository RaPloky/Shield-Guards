using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectAllGuards : MonoBehaviour
{

    [SerializeField] private bool isBonusEnabled;

    public bool IsBonusEnabled
    {
        get => isBonusEnabled;
        set => isBonusEnabled = value;
    }
}
