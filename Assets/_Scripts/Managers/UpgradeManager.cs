using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI creditsCount;
    [SerializeField] private TextMeshProUGUI chargingPrice;
    [SerializeField] private TextMeshProUGUI destroyingPrice;
    [SerializeField] private TextMeshProUGUI protectionPrice;

    [SerializeField, Range(0, 100000)] private float startUpgradePrice;
    [SerializeField, Range(1, 5)] private float nextUpgradeMultiplier;

    private GlitchAnimationController _glitchController;

    public static string ChargingBonusLvlPref => "ChargingLvl";
    public static string DemolitionBonusLvlPref => "DemolitionLvl";
    public static string ProtectionBonusLvlPref => "ProtectionLvl";
    public static string EnergyPref => "EnergyDepletedCount";

    public int ChargingBonusLvl => PlayerPrefs.GetInt(ChargingBonusLvlPref, 0);
    public int DemolitionBonusLvl => PlayerPrefs.GetInt(DemolitionBonusLvlPref, 0);
    public int ProtectionBonusLvl => PlayerPrefs.GetInt(ProtectionBonusLvlPref, 0);

    public IDictionary<int, float> ChargingEffectValues { get; private set; }
    public IDictionary<int, float> DemolitionEffectValues { get; private set; }
    public IDictionary<int, float> ProtectionEffectValues { get; private set; }

    public IDictionary<int, int> ChargingGainValues { get; private set; }
    public IDictionary<int, int> DemolitionGainValues { get; private set; }
    public IDictionary<int, int> ProtectionGainValues { get; private set; }

    public int[] UpgradesPrices { get; private set; }

    public float CurrChargeEffectValue => ChargingEffectValues[ChargingBonusLvl];
    public float CurrDemolitionEffectValue => DemolitionEffectValues[DemolitionBonusLvl];
    public float CurrProtectionEffectValue => ProtectionEffectValues[ProtectionBonusLvl];

    public int CurrChargeGoalValue => ChargingGainValues[ChargingBonusLvl];
    public int CurrDemolitionGoalValue => DemolitionGainValues[DemolitionBonusLvl];
    public int CurrProtectionGoalValue => ProtectionGainValues[ProtectionBonusLvl];

    public int EnergyValue
    {
        get => PlayerPrefs.GetInt(EnergyPref, 0);
        set => PlayerPrefs.SetInt(EnergyPref, value);
    }

    public int LevelsLimit => 7;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpgradesPrices = GetUpgradePrices(startUpgradePrice, nextUpgradeMultiplier);
        AssignEffectValues();
        AssignGainValues();        

        UpdateCreditsCount();
        UpdateUpgradePrices();
    }

    private void Start()
    {
        _glitchController = GlitchAnimationController.Instance;
    }

    private Dictionary<int, float> AssignBonusEffectValues(float minBonusEffectValue, float effectIncrement)
    {
        var valuesDict = new Dictionary<int, float>();
        valuesDict.Add(0, (int)minBonusEffectValue);

        for (int i = 1; i <= LevelsLimit; i++)
            valuesDict.Add(i, minBonusEffectValue += effectIncrement);

        return valuesDict;
    }

    private Dictionary<int, int> AssignBonusGainValues(int gainDecrement, BonusGoal bonusType)
    {
        var valuesDict = new Dictionary<int, int>();
        int startGoalValue = 0;
        
        switch (bonusType)
        {
            case BonusGoal.Charging:
                startGoalValue = GameManager.DefaultChargingGoal;
                break;
            case BonusGoal.Demolition:
                startGoalValue = GameManager.DefaultDemolitionGoal;
                break;
            case BonusGoal.Protection:
                startGoalValue = GameManager.DefaultProtectionGoal;
                break;
        }
        // Add default value as first:
        valuesDict.Add(0, startGoalValue);

        for (int i = 1; i <= LevelsLimit; i++)
            valuesDict.Add(i, startGoalValue -= gainDecrement);

        return valuesDict;
    }

    private int[] GetUpgradePrices(float startPrice, float multiplier)
    {
        int[] prices = new int[LevelsLimit];

        prices[0] = (int)startPrice;

        for (int priceIndex = 1; priceIndex < LevelsLimit; priceIndex++)
            prices[priceIndex] = (int)(prices[priceIndex - 1] * multiplier);

        return prices;
    }

    private void UpgradeBonus(int bonuslvl, string bonusPref)
    {
        int nextLvl = bonuslvl + 1;
        if (nextLvl > UpgradesPrices.Length)
            return;

        int nextUpgradeCost = UpgradesPrices[nextLvl - 1];
        if (!IsEnoughEnergyToUpgrade(nextUpgradeCost))
            return;

        _glitchController.PlayStrongScan();
        PlayerPrefs.SetInt(bonusPref, nextLvl);
        PlayerPrefs.SetInt(EnergyPref, EnergyValue -= nextUpgradeCost);
        UpdateCreditsCount();
        EventManager.SendOnBonusUpgraded();
    }

    public void UpgradeCharging() => UpgradeBonus(ChargingBonusLvl, ChargingBonusLvlPref);
    public void UpgradeDemolition() => UpgradeBonus(DemolitionBonusLvl, DemolitionBonusLvlPref);
    public void UpgradeProtection() => UpgradeBonus(ProtectionBonusLvl, ProtectionBonusLvlPref);

    private bool IsEnoughEnergyToUpgrade(int nextUpgradeCost) => EnergyValue >= nextUpgradeCost;
    private void UpdateCreditsCount() => creditsCount.text = "$" + EnergyValue;
    private string GetPrice(float price) => price.Equals(UpgradesPrices[^1]) ? "maxed, enjoy!" : "$" + price;

    public void UpdateUpgradePrices()
    {
        chargingPrice.text = GetPrice(UpgradesPrices[ChargingBonusLvl]);
        destroyingPrice.text = GetPrice(UpgradesPrices[DemolitionBonusLvl]);
        protectionPrice.text = GetPrice(UpgradesPrices[ProtectionBonusLvl]);
    }

    public int GetNextLvlPrice(int priceLvl) => UpgradesPrices[priceLvl];

    private void AssignEffectValues()
    {
        ChargingEffectValues = AssignBonusEffectValues(1000, 250);
        DemolitionEffectValues = AssignBonusEffectValues(3, 1);
        ProtectionEffectValues = AssignBonusEffectValues(5, 0.5f);
    }

    private void AssignGainValues()
    {
        ChargingGainValues = AssignBonusGainValues(1000, BonusGoal.Charging);
        DemolitionGainValues = AssignBonusGainValues(2, BonusGoal.Demolition);
        ProtectionGainValues = AssignBonusGainValues(3, BonusGoal.Protection);
    }

    #region "Bonus Descriptions"
    public string ChargingDescription => $"Charges all guards by {CurrChargeEffectValue}";
    public string ChargingWayToGain => $"Gains by charging guards for {CurrChargeGoalValue}";

    public string DemolitionDescription => $"Destroys enemies and freeze their appearance for {CurrDemolitionEffectValue}s";
    public string DemolitionWayToGain => $"Gains by destroying {CurrDemolitionGoalValue} enemies";

    public string ProtectionDescription => $"Immune to energy lose by {CurrProtectionEffectValue}s";
    public string ProtectionWayToGain => $"Gains by surviving {CurrProtectionGoalValue}s";


    public string ChargingNextLvlDesc
    { 
        get
        {
            if (ChargingBonusLvl + 1 >= LevelsLimit)
                return "Bonus maxed";
            else
                return $"New charging amount is {ChargingEffectValues[ChargingBonusLvl + 1]}";
        }
    }
    public string ChargingNextLvlCondition
    {
        get
        {
            if (ChargingBonusLvl + 1 >= LevelsLimit)
                return string.Empty;
            else
                return $"Energy amount to gain bonus: {ChargingGainValues[ChargingBonusLvl + 1]}";
        }
    }

    public string DemolitionNextLvlDesc { 
        get {
            if (DemolitionBonusLvl + 1 >= LevelsLimit)
                return "Bonus maxed";
            else
                return $"New enemies appear freeze is {DemolitionEffectValues[DemolitionBonusLvl + 1]}s";
        } 
    } 
    public string DemolitionNextLvlCondition 
    {
        get
        {
            if (DemolitionBonusLvl + 1 >= LevelsLimit)
                return string.Empty;
            else
                return $"Enemies destroyed to gain bonus: {DemolitionGainValues[DemolitionBonusLvl + 1]}";
        }
    } 

    public string ProtectionNextLvlDesc {
        get
        {
            if (ProtectionBonusLvl + 1 >= LevelsLimit)
                return "Bonus maxed";
            else
                return $"New immune time is {ProtectionEffectValues[ProtectionBonusLvl + 1]}s";
        }
    }
    public string ProtectionNextLvlCondition {
        get
        {
            if (ProtectionBonusLvl + 1 >= LevelsLimit)
                return string.Empty;
            else
                return $"Seconds to survive to gain bonus: {ProtectionGainValues[ProtectionBonusLvl + 1]}";
        }
    }
    #endregion
}
