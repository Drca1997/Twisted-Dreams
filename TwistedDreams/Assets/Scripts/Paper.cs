using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{
    // Start is called before the first frame update

    
    public GameObject[] cameras;
    private int currentCameraIndex;
    private int nPieces;
    private logSystem logSys;
    private DialogSystem dialogSystem;

    private void Awake()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        dialogSystem.ActivateDialog(true);
    }
    private void Start()
    {
        logSys = gameObject.GetComponentInChildren<logSystem>();
        currentCameraIndex = 0;
        nPieces = 0;

        //Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
        logSys.LoadLog();
    }

    //QUANDO ISTO TIVER SAIDA, METER O SEGUINTE QD FOR PARA AVANÇAR PARA O PROXIMO NIVEL:
    //logSys.SaveLog();
    //SE FOR PARA VOLTAR PARA O TUTORIAL:
    //logSys.clearPrefs();

    public void SwitchCam()
    {
        currentCameraIndex++;
        Debug.Log("C button has been pressed. Switching to the next camera");
        if (currentCameraIndex < cameras.Length)
        {
            cameras[currentCameraIndex - 1].gameObject.SetActive(false);
            cameras[currentCameraIndex].gameObject.SetActive(true);
            Debug.Log("Camera with name: " + cameras[currentCameraIndex].name + ", is now enabled");
        }
        else
        {
            cameras[currentCameraIndex - 1].gameObject.SetActive(false);
            currentCameraIndex = 0;
            cameras[currentCameraIndex].gameObject.SetActive(true);
            Debug.Log("Camera with name: " + cameras[currentCameraIndex].name + ", is now enabled");
        }
     
    }

    public void PiecePickUP()
    {
        nPieces++;
    }


    public int Picked()
    {
        return nPieces;
    }


}
