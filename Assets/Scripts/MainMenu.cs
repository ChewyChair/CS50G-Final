using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayButton()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void TestButton()
    {
        SceneManager.LoadScene("Test Scene");
    }

    public void RetryButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
