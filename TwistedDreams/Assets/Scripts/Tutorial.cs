using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private int startDialogTime;
    [SerializeField]
    private float startedWalkingTime;
    private bool startedWalking;
    [Tooltip("Time To Trigger Dialog If Started Walking (TTDISW)")]
    [SerializeField]
    private float tttdisw;
    public bool dialogStarted = false;
    public TMPro.TextMeshPro text;
    [SerializeField]
    [Tooltip("Move-Text-to-Disappear-After-Started-Walking Time (MTDASWT)")]
    private float mtdaswt;
    public TMPro.TextMeshPro porta_prompt;

    private void Start()
    {
       
        startedWalking = false;
    }

    private void Update()
    {
        if (Time.time > startDialogTime || (Time.time - startedWalkingTime > tttdisw && startedWalking))
        {
            gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog(true);
            if (text != null)
            {
                Destroy(text);
            }
        }
        if (!startedWalking && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            startedWalkingTime = Time.time;
            startedWalking = true;
            
        }
        if (startedWalking && Time.time - startedWalkingTime > mtdaswt)
        {
            Destroy(text);
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
        porta.GetComponent<BoxCollider>().enabled = false;
        porta.GetComponentInChildren<cakeslice.Outline>().enabled = false;
        porta_prompt.SetText("");
        
        
    }

    
    
    
}
