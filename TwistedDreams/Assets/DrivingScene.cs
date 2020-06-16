using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;
public class DrivingScene : MonoBehaviour
{
    public GameObject C;
    public GameObject car;
    public GameObject[] checkpoints;
    private int current_checkpoint;
    private DialogSystem dialogSystem;
    private logSystem logSys;
    public float time;
    public TMPro.TextMeshPro countdown;
    private bool Done;
    public float MinHeighLimit;
    public TextAsset final;
    public TextAsset fail;
    public TextAsset time_fail;
    [SerializeField]
    private int wait_finish;
    public Store_Time variable;
    public Text label;

    private void Awake()
    {
        logSys = gameObject.GetComponentInChildren<logSystem>();
        dialogSystem = FindObjectOfType<DialogSystem>();
        logSys.LoadLog();
        label.text = "Checkpoints Completed: 0/12";
    }
    // Start is called before the first frame update
    void Start()
    {
        current_checkpoint = 0;
        dialogSystem.ActivateDialog(false);
        Done = false;
        wait_finish = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (wait_finish == 0 && dialogSystem.Is_Dialog_Finished())
        {
            car.GetComponent<CarUserControl>().enabled = true;
            wait_finish++;
            Debug.Log("FINISH FIRST DIALOG");
            
        }
        else if (wait_finish == 3 && dialogSystem.Is_Dialog_Finished()) //Sucesso
        {
            variable.time = time;
            logSys.SaveLog();
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("CamsLevel");
        }
        else if(wait_finish == 2 && dialogSystem.Is_Dialog_Finished())  // Caiu do Plano de Jogo
        {
            logSys.clearPrefs();
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("Tutorial");
        }
        else if(wait_finish == 4 && dialogSystem.Is_Dialog_Finished()) // Tempo chegou ao fim
        {
            //Load PrecipicioFail
            logSys.clearPrefs();
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("Tutorial");
        }
        time -= Time.deltaTime;
        Update_Timer();
        if (time <= 0 && wait_finish != 4)
        {

            Destroy(countdown);
            wait_finish = 4;
            dialogSystem.ReStart(time_fail, (PlayerPrefs.GetInt("AutoDialog") == 1));
            dialogSystem.ActivateDialog(false);
            //Application.Quit();
        }

        if (Fall_Off_World() && current_checkpoint < 11 && wait_finish != 2)
        {
            //Good Job, Sarah
            dialogSystem.ReStart(fail, (PlayerPrefs.GetInt("AutoDialog") == 1));
            dialogSystem.ActivateDialog(false);
            wait_finish = 2;
        }

        
    }

    public void Update_Timer()
    {

        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        countdown.SetText(minutes + ':' + seconds);
    }

    public void AdvanceCheckpoint(GameObject checkpoint_reached)
    {
        checkpoint_reached.SetActive(false);
        current_checkpoint++;
        Update_Label(current_checkpoint);
        if (current_checkpoint < checkpoints.Length)
            checkpoints[current_checkpoint].SetActive(true);
        else
        {
            dialogSystem.ReStart(final, (PlayerPrefs.GetInt("AutoDialog") == 1));
            dialogSystem.ActivateDialog(false);
            wait_finish = 3;
        }

           
    }

    public void Update_Label(int completed)
    {
        string texto = "Checkpoints Completed: " + completed.ToString() + "/12"; 
        label.text = texto;
    }

    public bool Fall_Off_World()
    {
        if (car.transform.position.y < -12f)
        {
            
            return true;
        }
        return false;
    }

    
}
