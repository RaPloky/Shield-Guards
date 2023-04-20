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
    private Vector3 _startPosition;
    private ProjectileBehavior _projBehaviour;

    private void Awake()
    {
        _meteorRb = GetComponent<Rigidbody>();
        _startPosition = relatedSpawner.transform.position;
        _projBehaviour = GetComponent<ProjectileBehavior>();

        SetGlitchController();
    }

    private void OnMouseDrag()
    {
        if (GameManager.IsGamePaused)
            return;

        SwipeVertical();
    }

    // Temporary method for testing on PC
    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused)
            return;

        StartCoroutine(DisableThatEnemy());
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
            }

        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            _stopTouch = false;
    }

    private void DragMeteor()
    {
        _meteorRb.AddForce(dragTo.position, ForceMode.Impulse);

        StartCoroutine(DisableThatEnemy());
    }

    public override IEnumerator DisableThatEnemy()
    {
        yield return new WaitForSeconds(0);

        _projBehaviour.PlayParticlesOnDisable();

        DisableMeteor();
        DisableDangerNotifications();

        EventManager.SendOnEnemyDisabled();
        EventManager.SendOnScoreUpdated(destructionReward);
    }

    private void DisableMeteor()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = _startPosition;
    }
}
