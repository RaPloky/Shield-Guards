using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] private TextMeshProUGUI energyDepletedCount;

    public static string ChargingBonusLvlPref => "ChargingLvl";
    public static string DemolitionBonusLvlPref => "DemolitionLvl";
    public static string ProtectionBonusLvlPref => "ProtectionLvl";

    public static string CurrencyPref = "CurrencyCount";

    public int ChargingBonusLvl => PlayerPrefs.GetInt(ChargingBonusLvlPref, 1);
    public int DemolitionBonusLvl => PlayerPrefs.GetInt(DemolitionBonusLvlPref, 1);
    public int ProtectionBonusLvl => PlayerPrefs.GetInt(ProtectionBonusLvlPref, 1);

    public IDictionary<int, int> ChargingValues;
    public IDictionary<int, float> DemolitionValues;
    public IDictionary<int, float> ProtectionValues;

    public int CurrentChargeValue => ChargingValues[ChargingBonusLvl];
    public float CurrentDemolitionValue => DemolitionValues[DemolitionBonusLvl];
    public float CurrentProtectionValue => ProtectionValues[ProtectionBonusLvl];
    public int CurrencyValue => PlayerPrefs.GetInt(CurrencyPref, 0);

    public readonly int levelsLimit = 7;
    private float _minBonusEffectValue;
    private float _effectIncrement;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        ChargingValues = AssignChargingValues();
        DemolitionValues = AssignDemolitionValues();
        ProtectionValues = AssignProtectionValues();

        if (energyDepletedCount != null)
            energyDepletedCount.text = PlayerPrefs.GetInt(CurrencyPref, 0).ToString();
    }
    #region "Assign values of bonuses"
    private Dictionary<int, int> AssignChargingValues()
    {
        var valuesDict = new Dictionary<int, int>();
        _minBonusEffectValue = 1000;
        _effectIncrement = 250;

        for (int i = 1; i <= levelsLimit; i++)
            valuesDict.Add(i, (int)(_minBonusEffectValue += _effectIncrement));

        return valuesDict;
    }

    private Dictionary<int, float> AssignDemolitionValues()
    {
        var valuesDict = new Dictionary<int, float>();
        _minBonusEffectValue = 3;
        _effectIncrement = 1;

        for (int i = 1; i <= levelsLimit; i++)
            valuesDict.Add(i, _minBonusEffectValue += _effectIncrement);

        return valuesDict;
    }

    private Dictionary<int, float> AssignProtectionValues()
    {
        var valuesDict = new Dictionary<int, float>();
        _minBonusEffectValue = 5;
        _effectIncrement = 0.5f;

        for (int i = 1; i <= levelsLimit; i++)
            valuesDict.Add(i, _minBonusEffectValue += _effectIncrement);

        return valuesDict;
    }
    #endregion

    public void UpgradeChargeBonus()
    {

    }
}
