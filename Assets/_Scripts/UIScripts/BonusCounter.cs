using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusCounter : MonoBehaviour
{
    [SerializeField] BonusManager bonus;
    private TextMeshProUGUI _countOfBonuses;

    private void Awake()
    {
        _countOfBonuses = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        _countOfBonuses.text = bonus.GetComponent<BonusManager>().bonusCounter.ToString();
    }
}
