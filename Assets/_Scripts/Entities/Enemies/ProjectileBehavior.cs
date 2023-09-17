using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] protected float speedFactor;
    [SerializeField] private int energyDamage;
    [SerializeField] private bool isFragileTrash; // bad naming, sorry me - it means "ifIsActualEnemy"
    [SerializeField] private bool isBackgroundEnemy;
    [SerializeField, Range(0f, 1f)] private float glitchStrength;
    [SerializeField] private ParticleSystem onDisableParticles;
    [SerializeField] protected Transform targetTrans;
    [SerializeField] private Transform parentSpawner;
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioSource collisionAS;

    protected Transform _thatTrans;
    protected Vector3 _startPos;

    private Guard _guard;
    private Meteor _thatReference;
    private DifficultyUpdate _difficultyManager;
    private GameObject _collisionGO;

    private void Awake()
    {
        SetData();
    }

    protected void SetData()
    {
        _thatTrans = transform;
        _startPos = parentSpawner.position;
        _difficultyManager = DifficultyUpdate.Instance;

        if (isFragileTrash)
            _thatReference = GetComponent<Meteor>();
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    protected virtual void MoveToTarget()
    {
        _thatTrans.position = Vector3.MoveTowards(_thatTrans.position, targetTrans.position, speedFactor * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collisionGO = collision.gameObject;
        _guard = _collisionGO.GetComponent<Guard>();

        if (_guard == null)
        {
            if (_collisionGO.CompareTag("Enemy") && isFragileTrash && !isBackgroundEnemy)
            {
                Ufo ufo = _collisionGO.GetComponent<Ufo>();
                Carrier carrier = _collisionGO.GetComponent<Carrier>();

                if (ufo != null)
                    StartCoroutine(ufo.DisableThatEnemy(false));
                else if (carrier != null)
                    StartCoroutine(carrier.DisableThatEnemy(false));
            }

            DisableThatProjectile(false);
            return;
        }
        _guard.ConsumptEnergy(energyDamage + (int)(energyDamage * _difficultyManager.EnemyDamageIncreasePercentage));
        
        DisableThatProjectile(false);
        PlayHitGlitchAnim();
    }

    private void DisableThatProjectile(bool destroyedByPlayer)
    {
        if (isFragileTrash)
        {
            StartCoroutine(_thatReference.DisableThatEnemy(destroyedByPlayer));
        }
        else
        {
            PlayParticlesOnDisable();

            gameObject.SetActive(false);
            gameObject.transform.SetPositionAndRotation(_startPos, Quaternion.identity);
        }
        PlayCollisionSound();
    }

    public void PlayParticlesOnDisable()
    {
        GameObject particles = Instantiate(onDisableParticles.gameObject, _thatTrans.position, _thatTrans.rotation);
        onDisableParticles.Play();
        Destroy(particles, onDisableParticles.main.duration);
    }

    private void PlayHitGlitchAnim()
    {
        GlitchAnimationController controller = GlitchAnimationController.Instance;
        if (isFragileTrash)
            controller.SingleDriftAndDigital(0.6f, 0.4f);
        else
            controller.PlayWeakScan();
    }

    private void PlayCollisionSound()
    {
        if (!isBackgroundEnemy)
            collisionAS.PlayOneShot(collisionSound);
    }
}
