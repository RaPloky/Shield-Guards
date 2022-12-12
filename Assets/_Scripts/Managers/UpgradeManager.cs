using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static string ChargingBonusLvl => "ChargingLvl";
    public static string DemolitionBonusLvl => "DemolitionLvl";
    public static string ProtectionBonusLvl => "ProtectionLvl";


    public IDictionary<int, int> ChargingValues;
    public IDictionary<int, float> DemolitionValues;
    public IDictionary<int, float> ProtectionValues;

    public int CurrentChargeValue => ChargingValues[PlayerPrefs.GetInt(ChargingBonusLvl)];
    public float CurrentDemolitionValue => DemolitionValues[PlayerPrefs.GetInt(DemolitionBonusLvl)];
    public float CurrentProtectionValue => ProtectionValues[PlayerPrefs.GetInt(ProtectionBonusLvl)];

    private readonly int _levelsLimit = 7;
    private float _minBonusEffectValue;
    private float _effectIncrement;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        ChargingValues = AssignChargingValues();
        DemolitionValues = AssignDemolitionValues();
        ProtectionValues = AssignProtectionValues();
    }
    #region "Assign values of bonuses"
    private Dictionary<int, int> AssignChargingValues()
    {
        var valuesDict = new Dictionary<int, int>();
        _minBonusEffectValue = 1000;
        _effectIncrement = 250;

        for (int i = 1; i <= _levelsLimit; i++)
            valuesDict.Add(i, (int)(_minBonusEffectValue += _effectIncrement));

        return valuesDict;
    }

    private Dictionary<int, float> AssignDemolitionValues()
    {
        var valuesDict = new Dictionary<int, float>();
        _minBonusEffectValue = 3;
        _effectIncrement = 1;

        for (int i = 1; i <= _levelsLimit; i++)
            valuesDict.Add(i, _minBonusEffectValue += _effectIncrement);

        return valuesDict;
    }

    private Dictionary<int, float> AssignProtectionValues()
    {
        var valuesDict = new Dictionary<int, float>();
        _minBonusEffectValue = 5;
        _effectIncrement = 0.5f;

        for (int i = 1; i <= _levelsLimit; i++)
            valuesDict.Add(i, _minBonusEffectValue += _effectIncrement);

        return valuesDict;
    }
    #endregion
}
