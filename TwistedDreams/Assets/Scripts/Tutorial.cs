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
    public GameObject C;
    private string trigger_sentence;
    private bool Anim_Done;

    private void Start()
    {
        gameObject.GetComponentInChildren<DialogSystem>().setMovable(true);
        startedWalking = false;
        trigger_sentence = "Well, you see... That would be kind of hard.";

        Anim_Done = false;
    }

    private void Update()
    {
        if (gameObject.GetComponentInChildren<DialogSystem>().is_active())
        {
       
            if (gameObject.GetComponentInChildren<DialogSystem>().GetCurrentLine().Contains(trigger_sentence) && !Anim_Done)
            {
                
                Camera_Animation();
            }
        }
        
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
        GameObject porta = GameObject.FindGameObjectWithTag("Door");
        Debug.Log("Acabou");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cakeslice.OutlineEffect>().enabled = true;
        GameObject.FindGameObjectWithTag("Phone").GetComponent<BoxCollider>().enabled = true;
        //GameObject.FindGameObjectWithTag("Door").GetComponent<BoxCollider>().enabled = true;
        GameObject.FindGameObjectWithTag("Phone").GetComponent<Interactable>().enabled = true;
        porta.AddComponent<Interactable>();
        porta.GetComponent<Interactable>().TextUI = porta_prompt;
        porta.GetComponent<Interactable>().enabled = true;
        this.enabled = false;
    }

    public void DoorAnimation(GameObject porta) {
        
        porta.GetComponent<Animator>().SetTrigger("Abrir");
        porta.GetComponent<BoxCollider>().enabled = true;
        porta.GetComponent<cakeslice.Outline>().enabled = false;
        porta_prompt.SetText("");
    }

    public void Camera_Animation()
    {
        C.GetComponent<Animator>().SetTrigger("LookDown");
        Anim_Done = true;
    }
}
