using System.Collections;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DestroyThatEnemy();

    [SerializeField] protected int destructionReward;
    
    protected GameObject appearAlarm;

    protected void ActivateAlarm()
    {
        appearAlarm.SetActive(true);
    }

    protected void DeactivateAlarm()
    {
        appearAlarm.SetActive(false);
    }
}
