using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject OptionsPanel;

    public void PlayGame()
    {
        GameObject.Find("Canvas").SetActive(false);
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
        OptionsPanel.SetActive(!OptionsPanel.activeSelf);
    }

    public void changeAutoDialog(bool new_value)
    {
        PlayerPrefs.SetInt("AutoDialog",new_value ? 1 : 0);
        // To retrieve
        // autoDialog = PlayerPrefs.GetInt("Test") == 1 ? true : false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
