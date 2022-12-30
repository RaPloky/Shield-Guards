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

    public static string EnergyPref = "EnergyDepletedCount";

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
    public int EnergyValue => PlayerPrefs.GetInt(EnergyPref, 0);

    public readonly int levelsLimit = 7;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        ChargingValues = AssignBonusValues(1000, 250);
        DemolitionValues = AssignBonusValues(3, 1);
        ProtectionValues = AssignBonusValues(5, 0.5f);

        if (energyDepletedCount != null)
            energyDepletedCount.text = PlayerPrefs.GetInt(EnergyPref, 0).ToString();
    }

    private Dictionary<int, float> AssignBonusValues(float minBonusEffectValue, float effectIncrement)
    {
        var valuesDict = new Dictionary<int, float>();

        for (int i = 1; i <= levelsLimit; i++)
            valuesDict.Add(i, (int)(minBonusEffectValue += effectIncrement));

        return valuesDict;
    }

    public void UpgradeChargeBonus(int bonuslvl)
    {
        int nextLvl = bonuslvl + 1;
        if (nextLvl >= UpgradesPrices.Length)
            return;

        int nextUpgradeCost = UpgradesPrices[nextLvl];
        if (!IsEnoughEnergyToUpgrade(nextUpgradeCost))
            return;
    }

    private bool IsEnoughEnergyToUpgrade(int nextUpgradeCost) => EnergyValue >= nextUpgradeCost;
}
