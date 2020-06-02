using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool blinking;
    [Tooltip("Tempo necessário para desbloquear Cena Galeria")]
    public float time_to_wait;
    private DialogSystem dialogSystem;
    public TextAsset change_of_mind;
    public GameObject hasPhone;
    private bool dialog_has_started;
    private int wait_finish;

    private void Start()
    {
        dialogSystem = gameObject.GetComponentInChildren<DialogSystem>();
        dialogSystem.setMovable(true);
        startedWalking = false;
        trigger_sentence = "Well, you see... That would be kind of hard.";
        blinking = false;
        Anim_Done = false;
        dialog_has_started = false;
        wait_finish = 0;
        
    }

    private void Update()
    {
        if (dialogSystem.is_active() && !Anim_Done)
        {
       
            if (dialogSystem.GetCurrentLine().Contains(trigger_sentence))
            {
                
                Camera_Animation();
            }
        }
        
        if (Time.time > startDialogTime || (Time.time - startedWalkingTime > tttdisw && startedWalking) && !dialog_has_started)
        {
            dialogSystem.ActivateDialog(true);
            if (text != null)
            {
                Destroy(text);
            }
            dialog_has_started = true;
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
        if (dialogSystem.Is_Dialog_Finished() && !blinking){
            
            Debug.Log("DS active: " + dialogSystem.active);
            GlowingObjects();
            blinking = true;
            wait_finish++;
        }
        if (blinking)
        {
            time_to_wait -= Time.deltaTime;
        }

        if (time_to_wait <= 0 && dialogSystem.Is_Dialog_Finished() && wait_finish == 1)
        {
           
            dialogSystem.ReStart(change_of_mind, false);
            dialogSystem.ActivateDialog(false);
            wait_finish ++;
            
            
        }
        if (wait_finish == 2 && dialogSystem.Is_Dialog_Finished())
        {
            FindObjectOfType<Head_Animations>().Close_Eyes_Anim("Galeria");
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
        
    }

    public void DoorAnimation(GameObject porta) {
        
        porta.GetComponent<Animator>().SetTrigger("Abrir");
        porta.GetComponent<BoxCollider>().enabled = true;
        porta.GetComponent<cakeslice.Outline>().enabled = false;
        porta_prompt.SetText("");
    }

    public void Camera_Animation()
    {
        C.GetComponent<Head_Animations>().Do_LookDown_Anim();
        Anim_Done = true;
    }
}
