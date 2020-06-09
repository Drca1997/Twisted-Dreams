using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logSystem : MonoBehaviour
{
    private List<string> log = new List<string>();
    public Text logText;

    public void add_log(string line)
    {
        log.Add(line);
    }

    public void update_log()
    {
        string text = "";
        foreach(string line in log)
        {
            if(!line.Contains("log: "))
            {
                text = text + "" + line;
            }
        }
        logText.text = text;
    }

    public void SaveLog()
    {
        //Debug.Log("Saving");
        int i = -1;
        foreach (string line in log)
        {
            i++;
            PlayerPrefs.SetString("logList" + i, line);
        }

        PlayerPrefs.SetInt("logListSize", i);
    }

    public void LoadLog()
    {
        //Debug.Log("Loading");
        if (PlayerPrefs.HasKey("logListSize"))
        {
            int size = PlayerPrefs.GetInt("logListSize");
            for (int i = 0; i <= size; i++)
            {
                log.Add(PlayerPrefs.GetString("logList" + i));
            }
        }
    }

    public void clearPrefs()
    {
        bool auto;
        if (PlayerPrefs.HasKey("AutoDialog"))
        {
            auto = (PlayerPrefs.GetInt("AutoDialog") == 1 ? true : false);
        }
        else
        {
            auto = false;
        }

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("AutoDialog", auto ? 1 : 0);
    }
}
