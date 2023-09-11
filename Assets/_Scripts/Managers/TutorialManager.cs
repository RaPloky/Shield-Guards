using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] popUps;

    private int _popUpIndex = 0;
    private int CurrentPopUpIndex { 
        get => _popUpIndex; 
        set => _popUpIndex = Mathf.Clamp(value, 0, popUps.Length); 
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
        ManagePopUps();
    }

    public void NextPopUp() => CurrentPopUpIndex++;

    private void ManagePopUps()
    {

    }
}
