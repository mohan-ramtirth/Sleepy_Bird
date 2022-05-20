using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Pause()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Debug.Log("Game Resumed");
    }
    public void Restart()
    {
        Time.timeScale = 1;
        Debug.Log("Game Restarted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Debug.Log("Game Exitted");
        Application.Quit();
    }
}
