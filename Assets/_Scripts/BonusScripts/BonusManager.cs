using UnityEngine;
using TMPro;

abstract public class BonusManager : MonoBehaviour
{
    public int bonusCount;
    [Range(0, 1)] 
    public float chanceToInstantiate;
    [SerializeField] protected TextMeshProUGUI bonusCountUI;

    abstract public void ActivateBonus();
    public void InstantiateBonus()
    {
        float rnd = Random.Range(0f, 1f);
        if (rnd < chanceToInstantiate)
            bonusCount++;
    }
    protected void UpdateBonusCount()
    {
        bonusCount--;
        bonusCountUI.text = "x" + bonusCount.ToString();
    }
}
