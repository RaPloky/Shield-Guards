using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private PlayOneShotSound osSound;
    public static TutorialManager Instance;

    private enum TutorialStages
    {
        // Every number should be the same as 
        // number in related PopUp_X object name
        Effect_1 = 4,
        Effect_2 = 13,
        Effect_3 = 15,
        Effect_4 = 16,
        Effect_5 = 17,
        TransitionToGameplay = 18,
        AddingEnergyToGuards = 30,
        TurnOnFullGuardHud = 33,
        NavigationButtonsAppear = 36,
        IntroducingScore = 44,
        IntroducingUfo = 54,
        IntroducingMeteor = 58,
        IntroducingMicronovas = 62,
        TurnOfSimulation = 67, // gameplay tutorial end
        TutorialEnd = 69
    }

    [Header("PopUp 18: Transition to gameplay")]
    [SerializeField] private Animator transitionAnim;

    [Header("PopUp 30: Adding energy to Guards")]
    [SerializeField] private List<Guard> guardsComponents;

    [Header("PopUp 33: Informing about other Guards")]
    [SerializeField] private List<GameObject> hudElementsToTurnOn;

    [Header("PopUp 36: Navigation between Guards")]
    [SerializeField] private List<GameObject> navigationButtons;

    [Header("PopUp 44: Introducing score")]
    [SerializeField] private List<GameObject> scoreObjects;

    [Header("PopUp 54: Introducing Tyrandid marines")]
    [SerializeField] private List<Spawner> ufoSpawners;

    [Header("PopUp 57: Introducing Meteor")]
    [SerializeField] private List<Spawner> meteorsSpawners;

    [Header("PopUp 62: Introducing Micronova")]
    [SerializeField] private List<Spawner> micronovaSpawners;

    private bool _completedFullCharge = false;
    private bool _completedCharginUpPhase = false;
    private int _enemySimulationsDestroyed = 0;
    private bool _simulationTresholdReached = false;

    [SerializeField] private int simulationsTresholdToProceed;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject goToMenuButton;

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
        EventManager.OnEnemyDestroyed += UpdateSimulationsDestroyedCount;
    }
    private void OnDisable()
    {
        EventManager.OnTutorialPopUpIndexChanged -= ManageTutorialStages;
        EventManager.OnEnergyValueChanged -= CompleteSingleChargingUpTutor;
        EventManager.OnEnergyValueChanged -= CompleteAllChargingUpTutor;
        EventManager.OnEnemyDestroyed -= UpdateSimulationsDestroyedCount;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Make tutorial unskippable at first launch:
        if (!IsTutorialCompletedEver())
            goToMenuButton.SetActive(false);

        // Makes it possible to go through the tutorial again:
        if (IsTutorialCompletedEver() && IsTutorialFinished())
        {
            // Off any visuals and off any input:
            Camera.main.gameObject.SetActive(false);
            eventSystem.SetActive(false);

            GameManager.Instance.ExitToMenu();
        }
    }

    private bool IsTutorialFinished()
    {
        string _tutorialFinishedStance = PlayerPrefs.GetString(GameManager.Instance.TutorialFinished_Pref, GameManager.Instance.TutorialNotFinishedStance);
        bool _isTutorialFinished = _tutorialFinishedStance == GameManager.Instance.TutorialFinishedStance;
        return _isTutorialFinished;
    }

    private bool IsTutorialCompletedEver()
    {
        string _tutorialCompletedEverStance = PlayerPrefs.GetString(GameManager.Instance.TutorialCompletedEver_Pref, GameManager.Instance.TutorialNotCompletedEverStance);
        bool _isTutorialCompletedEver = _tutorialCompletedEverStance == GameManager.Instance.TutorialCompletedEverStance;
        return _isTutorialCompletedEver;
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

        osSound.PlayClip();
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

    private void UpdateSimulationsDestroyedCount()
    {
        if (!_simulationTresholdReached)
            _enemySimulationsDestroyed++;

        if (Mathf.Approximately(_enemySimulationsDestroyed, simulationsTresholdToProceed) && !_simulationTresholdReached)
        {
            _simulationTresholdReached = true;
            _enemySimulationsDestroyed = 0;

            DeactivateEnemySpawners();
            CurrentPopUpIndex++;
        }
    }

    private void DeactivateEnemySpawners()
    {
        static void Deactivate(List<Spawner> spawners) {
            foreach (Spawner spawner in spawners)
            {
                spawner.LaunchChance = 0;
                spawner.PrefabToOperate.GetComponent<Enemy>().DisableEnemy(false);
            }
        }

        Deactivate(ufoSpawners);
        Deactivate(meteorsSpawners);
        Deactivate(micronovaSpawners);
    }

    public void CompleteTutorial()
    {
        PlayerPrefs.SetString(GameManager.Instance.TutorialFinished_Pref, GameManager.Instance.TutorialFinishedStance);
        PlayerPrefs.SetString(GameManager.Instance.TutorialCompletedEver_Pref, GameManager.Instance.TutorialCompletedEverStance);
    }
    #endregion
    private void ManageTutorialStages()
    {
        switch ((TutorialStages)CurrentPopUpIndex)
        {
            default:
                break;

            #region "Effects"
            case TutorialStages.Effect_1:
                GlitchAnimationController.Instance.PlayWeakScan();
                break;

            case TutorialStages.Effect_2:
                GlitchAnimationController.Instance.PlayStrongScan();
                break;

            case TutorialStages.Effect_3:
                GlitchAnimationController.Instance.PlayStrongScan();
                break;

            case TutorialStages.Effect_4:
                GlitchAnimationController.Instance.SingleDriftAndDigital();
                break;

            case TutorialStages.Effect_5:
                GlitchAnimationController.Instance.SingleDriftAndDigital();
                break;
            #endregion

            case TutorialStages.TransitionToGameplay:
                GlitchAnimationController.Instance.SingleDriftAndDigital();
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

            case TutorialStages.NavigationButtonsAppear:
                foreach (GameObject navButton in navigationButtons)
                {
                    navButton.SetActive(true);
                    navButton.GetComponent<Button>().interactable = false;
                }
                break;

            // Firstly buttons activates non-interactable
            // and on next stage they become interactable so here's '+1':
            case TutorialStages.NavigationButtonsAppear + 1:
                foreach (GameObject navButton in navigationButtons)
                    navButton.GetComponent<Button>().interactable = true;
                break;

            case TutorialStages.IntroducingScore:
                foreach (GameObject scoreObj in scoreObjects)
                    scoreObj.SetActive(true);
                break;

            case TutorialStages.IntroducingUfo:
                _simulationTresholdReached = false;
                foreach (Spawner spawner in ufoSpawners)
                    spawner.LaunchChance = 1;
                break;

            case TutorialStages.IntroducingMeteor:
                _simulationTresholdReached = false;
                foreach (Spawner spawner in meteorsSpawners)
                    spawner.LaunchChance = 1;
                break;

            case TutorialStages.IntroducingMicronovas:
                _simulationTresholdReached = false;
                foreach (Spawner spawner in micronovaSpawners)
                    spawner.LaunchChance = 1;
                break;

            case TutorialStages.TurnOfSimulation:
                transitionAnim.SetTrigger("Gameplay_Out");
                break;

            case TutorialStages.TutorialEnd:
                CompleteTutorial();
                GameManager.Instance.ExitToMenu();
                break;
        }
    }
}
