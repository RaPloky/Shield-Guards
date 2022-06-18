using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        PauseMenu.isGamePaused = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
