using System.Collections;
using UnityEngine;

public class Meteor : Enemy
{
    protected ProjectileBehavior _projBehaviour;

    private void Awake()
    {
        SetData();
    }

    protected void SetData()
    {
        _difficultyManager = DifficultyUpdate.Instance;

        _startHealth = Health;
        _startPosition = relatedSpawner.transform.position;
        _projBehaviour = GetComponent<ProjectileBehavior>();

        SetGlitchController();
    }

    private void OnEnable()
    {
        ResetSoundObjBool();
        UpdateHealthBar();
        PlayOneShotSound(enableSound, ownAudioSource);
    }

    public override IEnumerator DisableThatEnemy(bool destroyedByPlayer)
    {
        yield return new WaitForSeconds(0);

        PlayDisableSound();
        _projBehaviour.PlayParticlesOnDisable();

        DisableMeteor();
        DisableDangerNotifications();

        if (destroyedByPlayer)
        {
            EventManager.SendOnEnemyDisabled();
            EventManager.SendOnScoreUpdated(destructionReward);
        }
    }

    private void DisableMeteor()
    {
        PauseDisableSoundObjMovement();
        gameObject.SetActive(false);
        gameObject.transform.position = _startPosition;
    }
}
