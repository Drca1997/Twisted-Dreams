using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private int passedTime;
    private int startDialogTime = 500;
    
    public bool dialogStarted = false;

    private void Start()
    {
        passedTime = 0;
    }

    private void Update()
    {
        if (passedTime > startDialogTime)
        {
            gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog();
        }
        else
        {
            passedTime++;
        }
        if (gameObject.GetComponentInChildren<DialogSystem>().Is_Dialog_Finished()){
            GlowingObjects();
        }
    }

    public void GlowingObjects()
    {     
        Debug.Log("Acabou");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cakeslice.OutlineEffect>().enabled = true;
        GameObject.FindGameObjectWithTag("Phone").GetComponent<BoxCollider>().enabled = true;
        GameObject.FindGameObjectWithTag("Door").GetComponent<BoxCollider>().enabled = true;
        GameObject.FindGameObjectWithTag("Phone").GetComponent<Interactable>().enabled = true;
        GameObject.FindGameObjectWithTag("Door").GetComponent<Interactable>().enabled = true;
        this.enabled = false;
    }

    public void DoorAnimation(GameObject porta) {
        
        porta.GetComponentInChildren<Animator>().SetTrigger("Abrir");
        
    }
    
    
}
