using System.Collections;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DisableThatEnemy();

    [SerializeField] protected int destructionReward;

    protected GlitchAnimationController _glitchController;
    public Spawner relatedSpawner;

    protected void DisableDangerNotifications()
    {
        relatedSpawner.DisableDanger();
    }

    protected void SetGlitchController() => _glitchController = GlitchAnimationController.Instance;
}
