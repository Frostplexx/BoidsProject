using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused;
    public GameObject pauseUI;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (paused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
        
    }

    void Unpause()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("unpaused");
        paused = false;
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("paused");
        paused = true;
    }

    void goToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
