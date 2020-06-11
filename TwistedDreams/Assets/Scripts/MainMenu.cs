﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject OptionsPanel;
    /*
    public void Start()
    {
        if(PlayerPrefs.HasKey("AutoDialog"))
        {
            GameObject.Find("AutoContinue").GetComponent<Toggle>().isOn = (PlayerPrefs.GetInt("AutoDialog") == 1 ? true : false);
        }
    }*/

    public void PlayGame()
    {
        GameObject.Find("MainMenuPanel").SetActive(false);
        StartCoroutine(Waiting());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime * 5);
    }

    public void invertMenu()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
        if (MenuPanel.activeSelf && PlayerPrefs.HasKey("AutoDialog"))
        {
            GameObject.Find("AutoContinue").GetComponent<Toggle>().isOn = (PlayerPrefs.GetInt("AutoDialog") == 1 ? true : false);
        }
        OptionsPanel.SetActive(!OptionsPanel.activeSelf);
    }

    public void changeAutoDialog(bool new_value)
    {
        PlayerPrefs.SetInt("AutoDialog",new_value ? 1 : 0);
        // To retrieve:
        // autoDialog = PlayerPrefs.GetInt("AutoDialog") == 1 ? true : false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // funçao para futuro uso de se guardarem apenas as prefs necessarias
    public void clearPrefs()
    {
        bool auto = GameObject.Find("AutoContinue").GetComponent<Toggle>().isOn;
        if (PlayerPrefs.HasKey("AutoDialog"))
        {
            auto = (PlayerPrefs.GetInt("AutoDialog") == 1 ? true : false);
        }

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("AutoDialog", auto ? 1 : 0);
    }
}
