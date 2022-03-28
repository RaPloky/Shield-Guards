using UnityEngine;

public class MeteorBehaviour : EnemyCommonValues
{
    public int addForceMultiplier = 200;
    public int inputPosAskedDifference = 250;
    public float speed = 15;

    private Rigidbody _meteorRb;
    private Vector3 _currInputPos;
    private Vector3 _prevInputPos;
    private bool _isMeteorDraggedAway = false;
    private bool _isCollisionDetected = false;
    private readonly int _invokeTime = 5;

    private void Awake()
    {
        SetOnAwake();
        spawnManagerTrans = gameObject.transform.parent.gameObject.GetComponent<Transform>();
        gameObject.transform.position = spawnManagerTrans.position;
        _meteorRb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnMouseOver()
    {
        // Reject to drag the same meteor again:
        if (_isMeteorDraggedAway || PauseMenu.isGamePaused) return;
        _prevInputPos = Input.mousePosition;
    }
    private void OnMouseDrag()
    {
        if (_isMeteorDraggedAway || PauseMenu.isGamePaused) return;

        _currInputPos = Input.mousePosition;
        DragMeteor();
    }
    private void DragMeteor()
    {
        if (PauseMenu.isGamePaused) return;
        if (Mathf.Abs(_currInputPos.y - _prevInputPos.y) >= inputPosAskedDifference)
        {
            _isMeteorDraggedAway = true;
            AddScore();
            Invoke(nameof(DestroyMeteorAfterDrag), _invokeTime);
        }
    }
    private void FixedUpdate()
    {
        if (!_isMeteorDraggedAway) 
        {
            _meteorRb.AddTorque(-transform.position.x * addForceMultiplier,
                -transform.position.y * addForceMultiplier, 0, ForceMode.Impulse);
            FollowSatelliteToDamageIt();
        }
        else
        {
            DragMeteorAway();
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Satellite") && !_isCollisionDetected)
        {
            // Avoiding case when energy level could be below the zero: 
            if (coll.gameObject.GetComponent<GameplayManager>().currentEnergyLevel > 0)
            {
                DoDamage(satelliteToDamage);
            }
            // Enabling to spawn meteor again:
            DestroyAndStartSpawn(spawnManagerTrans);
            _isCollisionDetected = true;
        }
        else if (coll.gameObject.CompareTag("UFO") && !_isCollisionDetected)
        {
            // Enabling to spawn meteor again:
            DestroyAndStartSpawn(spawnManagerTrans);
            // Enabling to spawn UFO again:
            DestroyAndStartSpawn(coll.gameObject.transform.parent.transform);
            _isCollisionDetected = true;
        }
        else
        {
            DestroyAndStartSpawn(spawnManagerTrans);
        }
    }
    private void FollowSatelliteToDamageIt()
    {
        // Following the player:
        _meteorRb.AddForce((satelliteToDamage.transform.position - transform.position) * speed,
            ForceMode.Impulse);
    }
    private void DragMeteorAway()
    {
        // Change position after dragging away:
        _meteorRb.AddForce((_currInputPos.x - _prevInputPos.x) * addForceMultiplier,
                (_currInputPos.y - _prevInputPos.y) * addForceMultiplier,
                (_currInputPos.z - _prevInputPos.z) * addForceMultiplier, ForceMode.Impulse);
    }
    private void DestroyMeteorAfterDrag()
    {
        DestroyAndStartSpawn(spawnManagerTrans);
    }
}
