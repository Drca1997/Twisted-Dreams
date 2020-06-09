using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.SceneManagement;
public class CliffEvent : MonoBehaviour
{

    public GameObject car;
    public GameObject C;
    [Tooltip("Quanto tempo o utilizador pode ficar a premir E ate que desbloqueie Final A. Se for mais que isso, ganha Final B")]
    public float time;
    public TMPro.TextMeshPro Prompt;
    private DialogSystem dialogSystem;
    private logSystem logSys;
    public TextAsset fail_text;
    public TextAsset middle_text;
    public TextAsset finalA_text;
    public TextAsset finalB_text;
    [SerializeField]
    private bool is_E_being_pressed;
    [SerializeField]
    private int wait_finish;
    private Rigidbody rb;
    private CarController car_controller;
    private CarAudio car_audio;

    private void Awake()
    {
        logSys = gameObject.GetComponentInChildren<logSystem>();
        dialogSystem = FindObjectOfType<DialogSystem>();
        dialogSystem.ActivateDialog(false);
        wait_finish = 0;
        is_E_being_pressed = false;
        rb = car.GetComponent<Rigidbody>();
        car_controller = car.GetComponent<CarController>();
        car_audio = car.GetComponent<CarAudio>();
        logSys.LoadLog();
    }
    private void Update()
    {
        State_Handler(); //Indica o que fazer em cada fase/estado da cena
        
        Car_Fall_Manager(); // verifica que dialogo iniciar quando carro cai do precipicio (fail ou success)
        
        E_Key_Handler();  //Lida com Pressionamentos da Tecla E
    }


    public void State_Handler()
    {
        if (dialogSystem.Is_Dialog_Finished() && wait_finish == 0) //primeiro dialogo acabou
        {
            wait_finish++;
            Prompt.SetText("HOLD E");
            
        }
        else if (dialogSystem.Is_Dialog_Finished() && wait_finish == 2) // dialogo do meio acabou
        {
            wait_finish++;
            Prompt.SetText("");
        }
        else if (dialogSystem.Is_Dialog_Finished() && wait_finish == 4) // dialogo final acabou
        {
            logSys.SaveLog();
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("Final_A");
        }
        else if (dialogSystem.Is_Dialog_Finished() && wait_finish == 5)
        {
            logSys.clearPrefs();
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("Tutorial");
        }
        else if (dialogSystem.Is_Dialog_Finished() && wait_finish == 6)
        {
            logSys.SaveLog();
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("Final_B");
        }

        if (time <= 0 && wait_finish == 3)
        {
            wait_finish = 6;
            dialogSystem.ReStart(finalB_text, true);
            dialogSystem.ActivateDialog(false);
        }
    }

    public void Car_Fall_Manager()
    {
        
        if (Fall_Off_World() && (wait_finish == 1 || wait_finish == 2))   //cai antes de dialogo do meio acabar = fail
        {
           
            //Debug.Log("Passou aqui");
            
            dialogSystem.brute_finish();
            dialogSystem.ReStart(fail_text, true);
            dialogSystem.ActivateDialog(false);
            wait_finish = 5;
        }
        else if (Fall_Off_World() && wait_finish == 3) //cai no fim do dialogo do meio = sucesso
        {
            dialogSystem.ReStart(finalA_text, true);
            dialogSystem.ActivateDialog(false);
            wait_finish++;
        }
    }


    public void E_Key_Handler()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            is_E_being_pressed = true;
            if (wait_finish >=1)
            {
                car_audio.enabled = false;
                ToggleCarAudioSources(false);
            }
            if (wait_finish == 1) //activa dialogo do meio
            {
                dialogSystem.ReStart(middle_text, true);
                dialogSystem.ActivateDialog(false);
                wait_finish++;
            }

        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            is_E_being_pressed = false;
            if (wait_finish >= 1 && wait_finish != 6)
            {
                car_audio.enabled = true;
                ToggleCarAudioSources(true);
            }
            
        }
        else if (Input.GetKey(KeyCode.E) && wait_finish == 3)
        {
            Debug.Log("COUNTING");
            time -= Time.deltaTime;
        }
    }

    public void FixedUpdate()
    {

        if (!is_E_being_pressed && wait_finish > 0 && wait_finish != 6)
        {
            Move_Carro();
        }
        else
        {
            Para_Carro();
        }

    }
    public void Move_Carro()
    {
        car_controller.Move(0f, 1f, 0f, 0f);
        //rb.AddForce(Vector3.up);
        
    }

    public void Para_Carro()
    {
        rb.velocity = (Vector3.zero);
    }

    public bool Fall_Off_World()
    {
       
        if (car.transform.position.y <= 25f)
        {
           
            return true;
        }
        return false;
    }

    public void ToggleCarAudioSources(bool toggle)
    {
        AudioSource []lista = car.GetComponents<AudioSource>();
        foreach(AudioSource source in lista)
        {
            source.enabled = toggle;
        }
    }
}
