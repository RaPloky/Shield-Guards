using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private enum TutorialStages
    {
        // Every number should be the same as 
        // number in related PopUp_X object name
        TransitionToGameplay = 18,
        AddingEnergyToGuards = 30,
        TurnOnFullGuardHud = 33,
        NavigationButtonAppear = 36,
    }

    [Header("PopUp 18: Transition to gameplay")]
    [SerializeField] private Animator transitionAnim;

    [Header("PopUp 30: Adding energy to Guards")]
    [SerializeField] private List<Guard> guardsComponents;

    [Header("PopUp 33: Informing about other Guards")]
    [SerializeField] private List<GameObject> hudElementsToTurnOn;

    private bool _completedCharginUpPhase = false;

    [Header("PopUp 36: Navigation between Guards")]
    [SerializeField] private List<GameObject> navigationButtons;

    private bool _completedFullCharge = false;

    [SerializeField] private GameObject[] popUps;

    private int _popUpIndex = 0;
    private int CurrentPopUpIndex { 
        get => _popUpIndex; 
        set => _popUpIndex = Mathf.Clamp(value, 0, popUps.Length); 
    }

    private void OnEnable()
    {
        EventManager.OnTutorialPopUpIndexChanged += ManageTutorialStages;
        EventManager.OnEnergyValueChanged += CompleteSingleChargingUpTutor;
        EventManager.OnEnergyValueChanged += CompleteAllChargingUpTutor;
    }
    private void OnDisable()
    {
        EventManager.OnTutorialPopUpIndexChanged -= ManageTutorialStages;
        EventManager.OnEnergyValueChanged -= CompleteSingleChargingUpTutor;
        EventManager.OnEnergyValueChanged -= CompleteAllChargingUpTutor;
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
    private void MakeGuardInteractable(Guard guard)
    {
        guard.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    // First guard only:
    private void CompleteSingleChargingUpTutor()
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
    // All guards:
    private void CompleteAllChargingUpTutor()
    {
        if (_completedFullCharge)
            return;

        foreach (Guard guard in guardsComponents)
        {
            if (!Mathf.Approximately(guard.Energy, guard.MaxEnergy))
                return;
        }
        CurrentPopUpIndex++;
        _completedFullCharge = true;
    }
    #endregion
    private void ManageTutorialStages()
    {
        switch ((TutorialStages)CurrentPopUpIndex)
        {
            default:
                break;

            case TutorialStages.TransitionToGameplay:
                transitionAnim.SetTrigger("Gameplay_In");
                break;

            case TutorialStages.AddingEnergyToGuards:
                foreach (Guard guard in guardsComponents)
                    MakeGuardInteractable(guard);
                break;

            case TutorialStages.TurnOnFullGuardHud:
                foreach (GameObject hudElement in hudElementsToTurnOn)
                    hudElement.SetActive(true);
                break;

            case TutorialStages.NavigationButtonAppear:
                foreach (GameObject navButton in navigationButtons)
                {
                    navButton.SetActive(true);
                    navButton.GetComponent<Button>().interactable = false;
                }
                break;

            // Firstly buttons activates non-interactable
            // and on next stage they become interactable so here's '+1':
            case TutorialStages.NavigationButtonAppear + 1:
                foreach (GameObject navButton in navigationButtons)
                    navButton.GetComponent<Button>().interactable = true;
                break;
        }
    }
}
