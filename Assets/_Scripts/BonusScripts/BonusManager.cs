using UnityEngine;

abstract public class BonusManager : MonoBehaviour
{
    public int bonusCounter;
    [Range(0, 1)] 
    public float chanceToInstantiate;

    abstract public void ActivateBonus();
    public void InstantiateBonus()
    {
        float rnd = Random.Range(0f, 1f);
        if (rnd < chanceToInstantiate)
        {
            bonusCounter++;
        }
    }
}
