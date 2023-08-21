using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Transform _trans;

    public bool AllowedToMove { get; set; }

    private void Start()
    {
        _trans = transform;
        AllowedToMove = true;
    }

    private void FixedUpdate()
    {
        if (AllowedToMove)
            _trans.position = target.position;
    }

    public void EnableMovePause(float pauseDuration) => StartCoroutine(PauseMove(pauseDuration));
    public void ResetBool() => AllowedToMove = true;

    private IEnumerator PauseMove(float pauseDuration)
    {
        AllowedToMove = false;
        yield return new WaitForSeconds(pauseDuration);
        AllowedToMove = true;
    }
}
