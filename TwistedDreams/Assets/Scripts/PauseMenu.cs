using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PMenu;

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        PMenu.SetActive(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        PMenu.SetActive(false);
        //carregar menu inicial;
    }
}
