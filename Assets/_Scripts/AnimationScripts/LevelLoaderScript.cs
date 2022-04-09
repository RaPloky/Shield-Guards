using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    [SerializeField] float transitionTime = 1f;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadGame(sceneName));
    }
    private IEnumerator LoadGame(string sceneName)
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
