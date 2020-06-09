using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quente_Frio : MonoBehaviour
{

    public GameObject player;
    public GameObject sarah_camera;
    private float distancia;
    private Som s;
    private Som bonk;
    private int waitfinish;
    public TextAsset final;
    public TextAsset away;
    public TextAsset no_phone;
    private bool sneak_away;
    private bool was_paused = false;
    private bool was_playing = false;
    private DialogSystem dialogSystem;
    private logSystem logSys;
    private string trigger_sentence;
    private GameObject phone;
    public GameObject CanvasPhone;
    public GameObject CanvasNoPhone;
    private void Awake()
    {
        phone = GameObject.FindGameObjectWithTag("HasPhone");
        
        //Debug.Log(phone);
        if (phone == null)
        {

            CanvasPhone.SetActive(false);
            CanvasNoPhone.SetActive(true);
            dialogSystem = gameObject.GetComponentInChildren<DialogSystem>();
            dialogSystem.ReStart(no_phone, (PlayerPrefs.GetInt("AutoDialog") == 1));
            
        }
        else
        {
            CanvasPhone.SetActive(true);
            CanvasNoPhone.SetActive(false);
            dialogSystem = gameObject.GetComponentInChildren<DialogSystem>();
            sneak_away = false;
            s = FindObjectOfType<AudioManager>().getSom("Quente_Frio");
            bonk = FindObjectOfType<AudioManager>().getSom("Bonk");
            
            trigger_sentence = "I have been trying to help you all along, Sarah.";
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        logSys = gameObject.GetComponentInChildren<logSystem>();
        dialogSystem.ActivateDialog(false);
        waitfinish = 0;
        logSys.LoadLog();
    }

    // Update is called once per frame
    void Update()
    {

        if (phone)
        {
            distancia = Vector3.Distance(player.transform.position, sarah_camera.transform.position);

            Debug.Log("DISTANCIA DA SARAH À CAMARA: " + distancia);
            if (waitfinish <= 1)
            {
                Debug.Log("entrou aqui");
                calcula_quente_frio(distancia);
            }

            if (dialogSystem.is_active() && sneak_away && dialogSystem.GetCurrentLine().Contains(trigger_sentence))
            {
                sarah_camera.GetComponent<Head_Animations>().Do_Horizontal_Headshake();
            }
            // if players goes against wall and no dialog is active -> depois trocar false pela cena de se bateu na parede
            if (!dialogSystem.is_active() && waitfinish == 0)
            {
                waitfinish++;
                s.source.Play();
                s.source.volume = 1;
            }

            if (!dialogSystem.is_active() && Is_Player_In_LOS() && distancia <= 2f && !dialogSystem.is_in_independent() && waitfinish == 1)
            {
                waitfinish++;
                dialogSystem.ReStart(final, (PlayerPrefs.GetInt("AutoDialog") == 1));
                dialogSystem.ActivateDialog(false);
                Debug.Log("entrou aqui");
                s.source.volume = 0;
            }

            if (!sneak_away && !dialogSystem.is_active() && distancia >= 11f && !Is_Player_In_LOS())
            {
                sneak_away = true;

                dialogSystem.ReStart(away, (PlayerPrefs.GetInt("AutoDialog") == 1));
                dialogSystem.ActivateDialog(true);

            }

            if (dialogSystem.Is_Dialog_Finished() && waitfinish == 2)
            {
                //Muda para a cena seguinte
                Debug.Log("ACABOU A CENA");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("CamsLevel");
                logSys.SaveLog();
                sarah_camera.GetComponent<Head_Animations>().Close_Eyes_Anim("ForestLevel");
            }

            if (player.GetComponent<PlayerInput>().is_paused)
            {
                if (s.source.isPlaying)
                {
                    was_playing = true;
                    s.source.Pause();
                    was_paused = true;
                }
            }
            else
            {
                if (was_paused && was_playing)
                {
                    s.source.UnPause();
                }
            }
        }
        else
        {
            if (dialogSystem.Is_Dialog_Finished())
            {
                //Reset ao Log
                logSys.clearPrefs();
                //Guarda Conquista de Final

                //Restart
                sarah_camera.GetComponent<Head_Animations>().Close_Eyes_Anim("Tutorial");
            }
        }
        
    }

    public void calcula_quente_frio(float distancia)
    {
        if (distancia <= 1.2f){
            s.source.volume = 1;
        }
        else if (distancia >= 10f)
        {
            s.source.volume = 0.01f;
        }
        else
        {
            s.source.volume = 1 - distancia * 12/ 100;
        }
    }

    public bool Is_Player_In_LOS()
    {
        var RaycastDirection = player.transform.position - sarah_camera.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(sarah_camera.transform.position, RaycastDirection, out hit))
        {
            if (hit.transform == player.transform)
            {
              
                return true;
            }
        }
        return false;
    }
}
