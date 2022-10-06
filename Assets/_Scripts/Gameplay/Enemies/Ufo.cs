using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    [SerializeField] private int health;

    private Transform _target;

    public Transform Target => _target;
    public int Health
    {
        get => health;
        set
        {
            health = value;
        }
    }

    private void Start()
    {
        FindClosestTarget();
    }

    private void FindClosestTarget()
    {
        int closestTargetIndex = 0;
        float distanceToClosestTarget = float.MaxValue;
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Satellite");

        if (targets == null)
            return;

        for (int i = 0; i < targets.Length; i++)
        {
            Transform targetTrans = targets[i].transform;
            float distanceBetweenSpawnerAndTarget = Vector3.Distance(transform.position, targetTrans.position);

            if (distanceBetweenSpawnerAndTarget < distanceToClosestTarget)
            {
                distanceToClosestTarget = distanceBetweenSpawnerAndTarget;
                closestTargetIndex = i;
            }
        }
        _target = targets[closestTargetIndex].transform;
    }

    private void FixedUpdate()
    {
        transform.LookAt(_target);
    }
}
