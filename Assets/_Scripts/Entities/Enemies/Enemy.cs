using System.Collections;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DestroyThatEnemy();

    [SerializeField] protected int destructionReward;

    protected Spawner _parentSpawner;
    protected GlitchAnimationController _glitchController;

    public Spawner ParentSpawner
    {
        get => _parentSpawner;
        set => _parentSpawner = value;
    }

    protected void DisableDangerNotifications()
    {
        _parentSpawner.DisableDanger();
    }

    protected void SetGlitchController() => _glitchController = GlitchAnimationController.Instance;
}
