using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusCounter : MonoBehaviour
{
    [SerializeField] private BonusManager bonus;
    private TextMeshProUGUI countOfBonuses;

    private void Awake()
    {
        countOfBonuses = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        countOfBonuses.text = bonus.GetComponent<BonusManager>().bonusCounter.ToString();
    }
}
