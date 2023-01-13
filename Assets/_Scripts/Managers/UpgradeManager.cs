using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI energyDepletedCount;
    [SerializeField, Range(50000, 100000)] private float startUpgradePrice;
    [SerializeField, Range(1, 5)] private float nextUpgradeMultiplier;

    public static string ChargingBonusLvlPref => "ChargingLvl";
    public static string DemolitionBonusLvlPref => "DemolitionLvl";
    public static string ProtectionBonusLvlPref => "ProtectionLvl";
    public static string EnergyPref => "EnergyDepletedCount";

    public int ChargingBonusLvl => PlayerPrefs.GetInt(ChargingBonusLvlPref, 1);
    public int DemolitionBonusLvl => PlayerPrefs.GetInt(DemolitionBonusLvlPref, 1);
    public int ProtectionBonusLvl => PlayerPrefs.GetInt(ProtectionBonusLvlPref, 1);

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

        if (energyDepletedCount != null)
            UpdatedEnergyDepletedCount();
    }

    private Dictionary<int, float> AssignBonusEffectValues(float minBonusEffectValue, float effectIncrement)
    {
        var valuesDict = new Dictionary<int, float>();

        for (int i = 1; i <= LevelsLimit; i++)
            valuesDict.Add(i, (int)(minBonusEffectValue += effectIncrement));

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

        PlayerPrefs.SetInt(bonusPref, nextLvl);
        PlayerPrefs.SetInt(EnergyPref, EnergyValue -= nextUpgradeCost);
        UpdatedEnergyDepletedCount();
        EventManager.SendOnBonusUpgraded();
    }

    public void UpgradeCharging() => UpgradeBonus(ChargingBonusLvl, ChargingBonusLvlPref);
    public void UpgradeDemolition() => UpgradeBonus(DemolitionBonusLvl, DemolitionBonusLvlPref);
    public void UpgradeProtection() => UpgradeBonus(ProtectionBonusLvl, ProtectionBonusLvlPref);

    private bool IsEnoughEnergyToUpgrade(int nextUpgradeCost) => EnergyValue >= nextUpgradeCost;
    private void UpdatedEnergyDepletedCount() => energyDepletedCount.text = EnergyValue.ToString();
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
    public string ChargingDescription => $"Instantly charges all guards by {CurrChargeEffectValue}";
    public string ChargingWayToGain => $"Gains by charging up guards for {CurrChargeGoalValue} on summary";

    public string DemolitionDescription => $"Destroys all enemies nearby and freeze their appearance by {CurrDemolitionEffectValue}s";
    public string DemolitionWayToGain => $"Gains by destroying {CurrDemolitionGoalValue} enemies";

    public string ProtectionDescription => $"Won't let use or lose any energy for {CurrProtectionEffectValue}s";
    public string ProtectionWayToGain => $"Gains by surviving {CurrProtectionGoalValue} seconds";


    public string ChargingNextLvlDesc => $"+{ChargingEffectValues[ChargingBonusLvl + 1] - CurrChargeEffectValue} to instant charging";
    public string ChargingNextLvlCondition => $"-{ChargingGainValues[ChargingBonusLvl + 1]} to gain bonus";

    public string DemolitionNextLvlDesc => $"+{DemolitionEffectValues[DemolitionBonusLvl + 1] - CurrDemolitionEffectValue} to enemies appear freeze";
    public string DemolitionNextLvlCondition => $"{DemolitionGainValues[DemolitionBonusLvl + 1]} less enemy to gain bonus";

    public string ProtectionNextLvlDesc => $"+{ProtectionEffectValues[ProtectionBonusLvl + 1] - CurrProtectionEffectValue} to shield life-time";
    public string ProtectionNextLvlCondition => $"{ProtectionGainValues[ProtectionBonusLvl + 1]} seconds less to survive to gain bonus";
    #endregion
}
