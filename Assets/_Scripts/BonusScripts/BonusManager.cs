using UnityEngine;

abstract public class BonusManager : MonoBehaviour
{
    [HideInInspector] 
    public bool[] needToInstantiate;
    public int bonusCounter;
    [Range(0, 100)] public int chanceToInstantiate;

    // Every bonus must have it's own implemention of this:
    abstract public void ActivateBonus();
    public void InstantiateBonus()
    {
        // Chance from 0% to 100%:
        int rnd = Random.Range(0, 100);
        if (needToInstantiate[rnd])
        {
            bonusCounter++;
        }
    }
    public void SetChanceToInstantiate()
    {
        // 100% for all chances:
        needToInstantiate = new bool[100];
        // For example, if chance was set to 30%, only 30 elems will have "true":
        for (byte i=0; i<chanceToInstantiate; i++)
        {
            needToInstantiate[i] = true;
        }
    }
}
