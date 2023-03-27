using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlitchString : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int stringsCount;
    [SerializeField, Range(10, 50)] private int singleStringLength;
    [SerializeField, Range(0, 0.2f)] private float updateDelay;

    private TextMeshProUGUI _thatText;
    private string[] _glitchStrings;
    private string[] _symbols;

    private void Awake()
    {
        _thatText = GetComponent<TextMeshProUGUI>();
        _glitchStrings = GetGlitchStrings();

        StartCoroutine(ChangeGlitchString());
    }

    private string[] GetGlitchStrings()
    {
        FillSymbols();
        string[] glitchStrings = new string[stringsCount];

        for (int i = 0; i < stringsCount; i++)
            glitchStrings[i] = GetSingleGlitchString();

        return glitchStrings;
    }

    private string GetSingleGlitchString()
    {
        string result = "";

        for (int i = 0; i < singleStringLength; i++)
            result += _symbols[Random.Range(0, _symbols.Length - 1)];

        return result;
    }

    private void FillSymbols()
    {
        _symbols = new string[] {"Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G",
            "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M", "1", "2", "3", "4", "5", "6", "7", "8",
            "9", "0", "!", "?", "#", "@", "%", "&", "*"};
    }

    private IEnumerator ChangeGlitchString()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateDelay);

            _thatText.text = _glitchStrings[Random.Range(0, _glitchStrings.Length - 1)];
        }
    }
}
