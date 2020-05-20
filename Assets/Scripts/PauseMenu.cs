using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool paused = false;
    public GameObject pauseMenu;
    public bool wait = false;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (paused == false && wait == false)
            {
                Pause();
                Wait(); 
            }
            else
            {
                Unpause();
                Wait();
            }
        }

    }

    public  void Unpause()
    {
        pauseMenu.SetActive(false);
        Debug.Log("unpaused");
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        paused = false; 
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Debug.Log("paused");
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        paused = true;
    }

    IEnumerator Wait()
    {
        wait = true;
        //yield on a new YieldInstruction that waits for 2 seconds.
        yield return new WaitForSeconds(2);
        wait = false;  
    }
}
