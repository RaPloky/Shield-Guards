using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GuardType
{
    None,
    Protector,
    Charger,
    Destroyer
}

public sealed class Guard : MonoBehaviour
{
    [SerializeField] private int energy;
    [SerializeField] private int consumption;
    [SerializeField, Range(0.5f, 1f)] private float consumptionDelay;
    [SerializeField] private int criticalEnergy;
    [SerializeField] private Bonus relatedBonus;
    [SerializeField] private CanvasGroup relatedDangerNotifications;
    [SerializeField] private List<ParticleSystem> onDisablePS;
    [SerializeField] private List<Animator> onDisableCanvasGroupAnimators;
    [SerializeField] private List<Animator> criticalEnergyAlerts;
    [SerializeField] private GuardType guardType; 
    
    private Animator _animator;
    private int _maxEnergy;
    private bool _isHaveEnergy;
    private Guard _thatGuard;
    private float _startConsumptionDelay;
    private bool _criticalEnergyActivated;

    public int MaxEnergy => _maxEnergy;
    public int Energy
    {
        get => energy;
        set
        {
            if (IsProtectBonusActivated && value < energy)
                return;

            energy = Mathf.Clamp(value, 0, _maxEnergy);
            EventManager.SendOnEnergyValueChanged();

            if (energy <= criticalEnergy && !_criticalEnergyActivated)
            {
                ActivateCriticalEnergyDanger("CriticalEnergyReached");
                _criticalEnergyActivated = true;
            }
            else if (energy > criticalEnergy && _criticalEnergyActivated)
            {
                ActivateCriticalEnergyDanger("CriticalEnergyLeaved");
                _criticalEnergyActivated = false;
            }

            if (Mathf.Approximately(energy, 0) && _isHaveEnergy)
                TurnOffGuard();
        }
    }
    public bool IsHaveEnergy => _isHaveEnergy;
    public bool IsProtectBonusActivated { get; set; }
    public Transform RelatedBonus => relatedBonus.transform;
    public float ConsumptionDelay
    {
        get => consumptionDelay;
        set => consumptionDelay = value;
    }
    public float StartConsumptionDelay => _startConsumptionDelay;
    public CanvasGroup RelatedDangerNotifications => relatedDangerNotifications;

    private void Awake()
    {
        IsProtectBonusActivated = false;
        _criticalEnergyActivated = false;
        _maxEnergy = energy;
        _isHaveEnergy = true;
        StartCoroutine(ConsumptEnergy());
        _thatGuard = this;
        _startConsumptionDelay = ConsumptionDelay;
        _animator = GetComponent<Animator>();
    }

    private IEnumerator ConsumptEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(consumptionDelay);
            ConsumptEnergy(consumption);
        }
    }

    private void TurnOffGuard()
    {
        DifficultyUpdate.Instance.SpecificDifficultyIncrease(guardType);
        DifficultyUpdate.Instance.RemoveGuardFromList(ref _thatGuard);
        EventManager.SendOnGuardDischarged();
        Invoke(nameof(DisableSelf), 5f);

        StopAllCoroutines();
        _isHaveEnergy = false;
        relatedBonus.DisableBonus();

        DisableParticles();
        DisableCanvasGroups();
        _animator.SetTrigger("Death");
    }

    public void AddEnergy(int energyAmount) => Energy += energyAmount;
    public void ConsumptEnergy(int energyConsumption) => Energy -= energyConsumption;

    private void DisableParticles()
    {
        for (int i = 0; i < onDisablePS.Count; i++)
            onDisablePS[i].gameObject.SetActive(false);
    }

    private void DisableCanvasGroups()
    {
        for (int i = 0; i < onDisableCanvasGroupAnimators.Count; i++)
        {
            onDisableCanvasGroupAnimators[i].SetTrigger("Disable");
        }
    }

    private void ActivateCriticalEnergyDanger(string triggerName)
    {
        for (int i = 0; i < criticalEnergyAlerts.Count; i++)
            criticalEnergyAlerts[i].SetTrigger(triggerName);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
