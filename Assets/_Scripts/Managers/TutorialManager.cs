using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopUpIndexes
{
    // Every number should be the same as 
    // number in related PopUp_X object name
    AddingEnergyToGuards = 31,
}

public class TutorialManager : MonoBehaviour
{
    [Header("PopUp 31: Adding energy to Guards")]
    [SerializeField] private List<Guard> guardsComponents;

    [SerializeField] private GameObject[] popUps;

    private int _popUpIndex = 0;
    private int CurrentPopUpIndex { 
        get => _popUpIndex; 
        set => _popUpIndex = Mathf.Clamp(value, 0, popUps.Length); 
    }

    private void OnEnable()
    {
        EventManager.OnTutorialPopUpIndexChanged += ManagePopUps;
    }
    private void OnDisable()
    {
        EventManager.OnTutorialPopUpIndexChanged -= ManagePopUps;
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
        EventManager.SendOnTutorialPopUpIndexChanger();
    }

    private void ActivateGuard(Guard guard)
    {
        guard.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void ManagePopUps()
    {
        switch ((PopUpIndexes)CurrentPopUpIndex)
        {
            default:
                break;

            case PopUpIndexes.AddingEnergyToGuards:
                foreach (Guard guard in guardsComponents)
                    ActivateGuard(guard);
                break;
        }
    }
}
