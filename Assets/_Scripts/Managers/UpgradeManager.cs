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

    public IDictionary<int, float> ChargingValues { get; private set; }
    public IDictionary<int, float> DemolitionValues { get; private set; }
    public IDictionary<int, float> ProtectionValues { get; private set; }

    public int[] UpgradesPrices { get; private set; }

    public float CurrentChargeValue => ChargingValues[ChargingBonusLvl];
    public float CurrentDemolitionValue => DemolitionValues[DemolitionBonusLvl];
    public float CurrentProtectionValue => ProtectionValues[ProtectionBonusLvl];
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
        ChargingValues = AssignBonusValues(1000, 250);
        DemolitionValues = AssignBonusValues(3, 1);
        ProtectionValues = AssignBonusValues(5, 0.5f);

        if (energyDepletedCount != null)
            UpdatedEnergyDepletedCount();
    }

    private Dictionary<int, float> AssignBonusValues(float minBonusEffectValue, float effectIncrement)
    {
        var valuesDict = new Dictionary<int, float>();

        for (int i = 1; i <= LevelsLimit; i++)
            valuesDict.Add(i, (int)(minBonusEffectValue += effectIncrement));

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

}
