using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] popUps;

    private int _popIndex;

    private void Update()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == _popIndex)
            {
                popUps[_popIndex].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }

        if (_popIndex == 0)
        {
            // if instruction done, then _popIndex++
        }
    }
}
