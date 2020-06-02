using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleDone : MonoBehaviour
{

    public GameObject[] pieces;

    void Start()
    {
        
        pieces = GameObject.FindGameObjectsWithTag("Piece");
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
        if(AllActive())
        {
            SceneManager.LoadScene("Paper");
        }
    }

}
