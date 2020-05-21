using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WonMenu : MonoBehaviour
{
    public GameObject wonMenu;
    public GameObject KeyReceiver;


    //Vector3.Distance(BoidsErschaffen.pl.transform.position, KeyReceiver.transform.position) <= 10f &&
    private void Update()
    {
        if (Score.won == true)
        {

            Pause();


        }

    }
    public void Unpause()
    {
        wonMenu.SetActive(false);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

    }

    public void Pause()
    {
        wonMenu.SetActive(true);
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
