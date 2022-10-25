using UnityEngine;
using System.Collections;

public class Ufo : Enemy
{
    [SerializeField] private int health;
    [SerializeField] private int damageToUfo;

    public Transform Target => _target;
    public int Health
    {
        get => health;
        set
        {
            health = (int)(Mathf.Clamp(value, 0, float.MaxValue));

            if (health <= 0)
                StartCoroutine(DestroyThatEnemy());
        }
    }

    private Transform _target;
    private Transform _thatTrans;

    private void Start()
    {
        _thatTrans = transform;
        _target = GetTargetFromSpawner();
        appearAlarm = GetAlarmGOFromSpawner();
        ActivateAlarm();
    }

    public override IEnumerator DestroyThatEnemy()
    {
        yield return new WaitForSeconds(0);
        DeactivateAlarm();
        Destroy(gameObject);
        
        EventManager.SendOnEnemyDestroyed();
        EventManager.SendOnScoreUpdated(destructionReward);
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused)
            return;

        DamageUfo();
    }

    private void FixedUpdate() => _thatTrans.LookAt(_target);
    private void DamageUfo() => Health -= damageToUfo;
    private Transform GetTargetFromSpawner() => _thatTrans.parent.GetComponent<Spawner>().Target;
    private GameObject GetAlarmGOFromSpawner() => _thatTrans.parent.GetComponent<Spawner>().AppearAlarm;
}
