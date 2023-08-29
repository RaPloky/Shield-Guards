using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Enemy : MonoBehaviour
{
    abstract public IEnumerator DisableThatEnemy();

    [SerializeField] protected int destructionReward;
    [SerializeField] protected int health;
    [SerializeField] protected int damageToEnemy;

    [SerializeField] protected List<Image> healthBarBg;
    [SerializeField] protected ParticleSystem onDamageParticles;

    [SerializeField] protected AudioSource ownAudioSource;
    [SerializeField] protected AudioClip enableSound;
    [SerializeField] protected AudioClip disableSound;
    [SerializeField] protected AudioSource onDisableAS;
    [SerializeField] protected FollowTarget onDisableAS_Obj;


    protected DifficultyUpdate _difficultyManager;
    protected GlitchAnimationController _glitchController;
    protected Vector3 _startPosition;
    protected int _startHealth;
    protected int _upgradedHealth;
    protected float DisableSoundDuration => disableSound.length;

    public Spawner relatedSpawner;

    public int Health
    {
        get => health;
        set
        {
            health = (int)Mathf.Clamp(value, 0, float.MaxValue);
            UpdateHealthBar();

            if (health <= 0)
                StartCoroutine(DisableThatEnemy());
        }
    }

    protected virtual void DisableDangerNotifications()
    {
        relatedSpawner.DisableDanger();
    }

    protected virtual void UpdateHealthBar()
    {
        if (healthBarBg == null || healthBarBg.Count == 0)
            return;

        for (int i = 0; i < healthBarBg.Count; i++)
            healthBarBg[i].fillAmount = (float)Health / _startHealth;
    }

    protected virtual void DamageEnemy()
    {
        if (IsGameOver())
            return;

        Health -= damageToEnemy - (int)(damageToEnemy * _difficultyManager.GuardAttackDecreasePercentage);

        if (onDamageParticles != null)
            onDamageParticles.Play();
    }

    private void OnMouseDown()
    {
        if (GameManager.IsGamePaused)
            return;

        DamageEnemy();
    }


    private bool IsGameOver() => Mathf.Approximately(_difficultyManager.ActiveGuards.Count, 0);
    protected void PlayOneShotSound(AudioClip clipToPlay, AudioSource source) => source.PlayOneShot(clipToPlay);

    protected void SetGlitchController() => _glitchController = GlitchAnimationController.Instance;
    protected void PlayDisableSound() => PlayOneShotSound(disableSound, onDisableAS);
    protected void PauseDisableSoundObjMovement() => onDisableAS_Obj.EnableMovePause(DisableSoundDuration * 3);
    protected void ResetSoundObjBool() => onDisableAS_Obj.ResetBool();
}
