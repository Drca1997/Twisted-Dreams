using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    
    private bool DialogFinished = false;
    private bool dialogStarted = false;
    

    private void Update()
    {

        GlowingObjects();
    }

    public void GlowingObjects()
    {
        if (gameObject.GetComponentInChildren<DialogSystem>().active && !dialogStarted)
        {
            dialogStarted = true;
        }
        else if (!gameObject.GetComponentInChildren<DialogSystem>().active && dialogStarted && !DialogFinished)
        {
            DialogFinished = true;
        }
        else if (DialogFinished)
        {

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cakeslice.OutlineEffect>().enabled = true;
            GameObject.FindGameObjectWithTag("Phone").GetComponent<BoxCollider>().enabled = true;
            GameObject.FindGameObjectWithTag("Door").GetComponent<BoxCollider>().enabled = true;
            GameObject.FindGameObjectWithTag("Phone").GetComponent<Interactable>().enabled = true;
            GameObject.FindGameObjectWithTag("Door").GetComponent<Interactable>().enabled = true;
            this.enabled = false;

        }


    }

    public void DoorAnimation(GameObject porta) {
        
        porta.GetComponentInChildren<Animator>().SetTrigger("Abrir");
        

    
    }
    
    
}
