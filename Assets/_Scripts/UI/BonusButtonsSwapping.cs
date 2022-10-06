using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusButtonsSwapping : MonoBehaviour
{
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private List<Transform> guards;
    [SerializeField] private List<GameObject> bonusButtons;

    private Transform _closestGuard;

    private void Start()
    {
        ArrangeBonusButtons();
    }

    private void ArrangeBonusButtons()
    {
        int closestTargetIndex = 0;
        float distanceToClosestGuard = float.MaxValue;

        if (guards == null)
            return;

        for (int i = 0; i < guards.Count; i++)
        {
            Transform guardTrans = guards[i];
            float distanceBetweenCameraAndGuard = Vector3.Distance(transform.position, guardTrans.position);

            if (distanceBetweenCameraAndGuard < distanceToClosestGuard)
            {
                distanceToClosestGuard = distanceBetweenCameraAndGuard;
                closestTargetIndex = i;
            }
        }
        _closestGuard = guards[closestTargetIndex];
    }
}
