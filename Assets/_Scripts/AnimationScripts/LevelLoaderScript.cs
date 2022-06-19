using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    [SerializeField] float transitionTime = 1f;
    [SerializeField] Animator controller;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadGame(sceneName));
    }
    private IEnumerator LoadGame(string sceneName)
    {
        Time.timeScale = 1f;
        controller.Play("OutMenu");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
