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
        UpdateHealthBar();
        PlayOneShotSound(enableSound, ownAudioSource);
    }

    public override IEnumerator DisableThatEnemy()
    {
        yield return new WaitForSeconds(0);

        PlayDisableSound();
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
