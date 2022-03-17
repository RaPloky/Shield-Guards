using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadGame(sceneName));
    }
    private IEnumerator LoadGame(string sceneName)
    {
        transition.SetTrigger("Start");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
