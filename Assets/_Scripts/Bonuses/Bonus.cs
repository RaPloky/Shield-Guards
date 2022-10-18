using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] protected bool isBonusEnabled;

    public bool IsBonusEnabled
    {
        get => isBonusEnabled;
        set => isBonusEnabled = value;
    }
}
