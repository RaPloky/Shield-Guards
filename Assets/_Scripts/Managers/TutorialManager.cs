using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopUpIndexes
{
    // Every number should be the same as 
    // number in related PopUp_X object name
    AddingEnergyToGuards = 30,
    TransitionToGameplay = 18,
}

public class TutorialManager : MonoBehaviour
{
    [Header("PopUp 18: Transition to gameplay")]
    [SerializeField] private Animator transitionAnim;
    [Header("PopUp 30/Common: Adding energy to Guards")]
    [SerializeField] private List<Guard> guardsComponents;
    private bool _completedCharginUpPhase = false;

    [SerializeField] private GameObject[] popUps;

    private int _popUpIndex = 0;
    private int CurrentPopUpIndex { 
        get => _popUpIndex; 
        set => _popUpIndex = Mathf.Clamp(value, 0, popUps.Length); 
    }

    private void OnEnable()
    {
        EventManager.OnTutorialPopUpIndexChanged += ManagePopUps;
        EventManager.OnEnergyValueChanged += CompleteChargingUpTutor;
    }
    private void OnDisable()
    {
        EventManager.OnTutorialPopUpIndexChanged -= ManagePopUps;
        EventManager.OnEnergyValueChanged -= CompleteChargingUpTutor;
    }

    private void Update()
    {
        for (int popUpIndex = 0; popUpIndex < popUps.Length; popUpIndex++)
        {
            if (popUpIndex == CurrentPopUpIndex)
            {
                popUps[CurrentPopUpIndex].SetActive(true);
            }
            else
            {
                popUps[popUpIndex].SetActive(false);
            }
        }
    }

    public void NextPopUp()
    {
        CurrentPopUpIndex++;
        EventManager.SendOnTutorialPopUpIndexChanged();
    }
    #region "Custom Instructions"
    private void ActivateGuard(Guard guard)
    {
        guard.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void CompleteChargingUpTutor()
    {
        foreach (Guard guard in guardsComponents)
        {
            if (Mathf.Approximately(guard.Energy, guard.MaxEnergy) && !_completedCharginUpPhase)
            {
                CurrentPopUpIndex++;
                _completedCharginUpPhase = true;
            }
        }
    }
    #endregion
    private void ManagePopUps()
    {
        switch ((PopUpIndexes)CurrentPopUpIndex)
        {
            default:
                break;

            case PopUpIndexes.TransitionToGameplay:
                transitionAnim.SetTrigger("Gameplay_In");
                break;
            case PopUpIndexes.AddingEnergyToGuards:
                foreach (Guard guard in guardsComponents)
                    ActivateGuard(guard);
                break;
        }
    }
}
