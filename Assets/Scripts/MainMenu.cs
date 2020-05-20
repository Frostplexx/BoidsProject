using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Debug.Log("AYYYYYYYY");
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
