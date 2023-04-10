using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private int energy;
    [SerializeField] private int consumption;
    [SerializeField, Range(0.5f, 1f)] private float consumptionDelay;
    [SerializeField] private Bonus relatedBonus;
    [SerializeField] private CanvasGroup relatedDangerNotifications;
    [SerializeField] private List<ParticleSystem> onDisablePS;
    [SerializeField] private Animator animator;

    [SerializeField] private Material bonusLight;
    [SerializeField] private Material engineLight;

    private int _maxEnergy;
    private bool _isHaveEnergy;
    private Guard _thatGuard;
    private float _startConsumptionDelay;

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
        EnableEnergyMaterials();

        IsProtectBonusActivated = false;
        _maxEnergy = energy;
        _isHaveEnergy = true;
        StartCoroutine(ConsumptEnergy());
        _thatGuard = this;
        _startConsumptionDelay = ConsumptionDelay;
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
        StopAllCoroutines();
        _isHaveEnergy = false;
        relatedBonus.DisableBonus();
        DifficultyUpdate.Instance.RemoveGuardFromList(ref _thatGuard);

        DisableParticles();
        DisableEnergyMaterials();
        EventManager.SendOnGuardDischarged();
    }

    public void AddEnergy(int energyAmount) => Energy += energyAmount;
    public void ConsumptEnergy(int energyConsumption) => Energy -= energyConsumption;

    private void DisableParticles()
    {
        foreach (var particle in onDisablePS)
            particle.Stop();
    }

    private void DisableEnergyMaterials()
    {
        bonusLight.DisableKeyword("_EMISSION");
        engineLight.DisableKeyword("_EMISSION");
    }

    private void EnableEnergyMaterials()
    {
        bonusLight.EnableKeyword("_EMISSION");
        engineLight.EnableKeyword("_EMISSION");
    }
}
