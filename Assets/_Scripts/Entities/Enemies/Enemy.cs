using System.Collections;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DestroyThatEnemy();

    [SerializeField] protected int destructionReward;

    protected void ActivateAlarm()
    {

    }

    protected void DeactivateAlarm()
    {

    }
}
