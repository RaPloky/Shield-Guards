using System.Collections;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DestroyThatEnemy();

    [SerializeField] protected int destructionReward;

    protected Spawner _parentSpawner;

    public Spawner ParentSpawner
    {
        get => _parentSpawner;
        set => _parentSpawner = value;
    }

    protected void DisableDangerNotifications()
    {
        _parentSpawner.DisableDanger();
    }
}
