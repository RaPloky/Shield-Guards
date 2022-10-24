using System.Collections;
using UnityEngine;

public class Meteor : Enemy
{
    [Header("Swipe control")]
    [SerializeField] private float swipeRange;
    [SerializeField] private float tapRange;

    [Header("Other")]
    [SerializeField] private Transform dragTo;

    private Rigidbody _meteorRb;
    private Vector2 _startTouchPosition;
    private Vector2 _currentPosition;
    private bool _stopTouch = false;

    private void Awake()
    {
        _meteorRb = GetComponent<Rigidbody>();
        appearAlarm = GetAlarmGOFromSpawner();
        ActivateAlarm();
    }

    private void OnMouseDrag()
    {
        SwipeVertical();
    }

    // Temporary method for testing on PC
    private void OnMouseDown()
    {
        StartCoroutine(DestroyThatEnemy());
    }

    private void SwipeVertical()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            _startTouchPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            _currentPosition = Input.GetTouch(0).position;
            Vector2 distance = _currentPosition - _startTouchPosition;

            if (!_stopTouch)
            {
                if (distance.y > swipeRange)
                {
                    DragMeteor();
                    _stopTouch = true;
                }
                //else if (distance.y < -swipeRange)
                //{
                //    DragMeteorDown();
                //    stopTouch = true;
                //}
            }

        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            _stopTouch = false;
    }

    private void DragMeteor()
    {
        _meteorRb.AddForce(dragTo.position, ForceMode.Impulse);

        StartCoroutine(DestroyThatEnemy());
    }

    public override IEnumerator DestroyThatEnemy()
    {
        yield return new WaitForSeconds(0);
        DeactivateAlarm();
        Destroy(gameObject);

        EventManager.SendOnEnemyDestroyed();
        EventManager.SendOnScoreUpdated(destructionReward);
    }

    private GameObject GetAlarmGOFromSpawner() => transform.parent.GetComponent<Spawner>().AppearAlarm;
}
