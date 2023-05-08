using System.Collections;
using UnityEngine;

public class Meteor : Enemy
{
    private ProjectileBehavior _projBehaviour;

    private void Awake()
    {
        _difficultyManager = DifficultyUpdate.Instance;

        _startHealth = Health;
        _startPosition = relatedSpawner.transform.position;
        _projBehaviour = GetComponent<ProjectileBehavior>();

        SetGlitchController();
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused)
            return;

        DamageEnemy();
    }

    private void OnEnable()
    {
        UpdateHealthBar();
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
