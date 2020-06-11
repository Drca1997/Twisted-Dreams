using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleDone : MonoBehaviour
{

    public GameObject[] pieces;
    private bool in_log;
    public GameObject LogCanvas;
    private logSystem logSys;

    private void Awake()
    {
        in_log = false;
    }

    void Start()
    {
        logSys = LogCanvas.GetComponent<logSystem>();
        pieces = GameObject.FindGameObjectsWithTag("Piece");
        logSys.LoadLog();
    }

    bool AllActive()
    {
        foreach (GameObject piece in pieces)
        {
            DragAndDrop script = piece.GetComponent<DragAndDrop>();
            if (!script.InRightPosition)
            {
                return false;
            }

        }
        return true;
    }

    public bool CheckObjStatus()
    {
        if (AllActive())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L)) {
            if (!in_log)
            {
                LogCanvas.transform.Find("Image").gameObject.SetActive(true);
                LogCanvas.transform.Find("ScrollArea").gameObject.SetActive(true);
                in_log = true;
                LogCanvas.transform.Find("LogTurnText").GetComponent<UnityEngine.UI.Text>().text = "  Esc/L - Go back";
                LogCanvas.GetComponent<logSystem>().update_log();
            }
            else
            {
                LogCanvas.transform.Find("Image").gameObject.SetActive(false);
                LogCanvas.transform.Find("ScrollArea").gameObject.SetActive(false);
                in_log = false;
                LogCanvas.transform.Find("LogTurnText").GetComponent<UnityEngine.UI.Text>().text = "  L - Check Log";
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && in_log)
        {
            LogCanvas.transform.Find("Image").gameObject.SetActive(false);
            LogCanvas.transform.Find("ScrollArea").gameObject.SetActive(false);
            in_log = false;
            LogCanvas.transform.Find("LogTurnText").GetComponent<UnityEngine.UI.Text>().text = "  L - Check Log";
        }

        if(AllActive())
        {
            logSys.SaveLog();
            SceneManager.LoadScene("Precipício");
        }
    }

}
