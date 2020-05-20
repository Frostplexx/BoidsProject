using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool paused;
    public GameObject pauseUI;

    void Update()
    {
        
    }

    void Unpause()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;

    }

    void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    void goToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
