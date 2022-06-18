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
        _countOfBonuses.text = "x"+bonus.GetComponent<BonusManager>().bonusCounter.ToString();
    }
}
