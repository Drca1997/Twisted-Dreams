using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamsLevel : MonoBehaviour
{
    private int bidoes_apanhados;
    public GameObject Canvas;
    private float time_left;
    private float time;
    public TMPro.TextMeshPro countdown;
    public TextAsset wall_break;
    private logSystem logSys;
    [Tooltip("Referencia ao Scriptableobject que guarda o tempo do Timer entre cenas")]
    public Store_Time variable;

    private void Awake()
    {
        logSys = gameObject.GetComponentInChildren<logSystem>();
        bidoes_apanhados = 0;
        gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog(false); 
        time = variable.time;
        Update_Timer();
        logSys.LoadLog();
    }


    public void Update()
    {
        if (!gameObject.GetComponentInChildren<DialogSystem>().is_active() && !gameObject.GetComponentInChildren<DialogSystem>().is_in_independent())
        {
            time -= Time.deltaTime;
            Update_Timer();
            if (time <= 0)
            {
                logSys.SaveLog();
                //Load PrecipicioFail
                Application.Quit();
            }
        }
    }

    public void Update_Timer()
    {
        
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        countdown.SetText(minutes + ':' + seconds);
    }

    public void NEED_MORE_GAS()
    {
        string[] lines = { "There's still more gas left to find!" , "It needs MORE GAS, SARAH!", "Hurry up, you're not finished. Find some more!"};
        string almost = "Almost there! One gas can left to go!";

        if (bidoes_apanhados == 4)
        {
            Canvas.GetComponent<DialogSystem>().independentDialog("???", almost);
        }
        else if(bidoes_apanhados == 0)
        {
            Canvas.GetComponent<DialogSystem>().independentDialog("???", "What did I just tell you? IT NEEDS GAS!");
        }
        else
        {
            int rand = Random.Range(0, lines.Length);
            Canvas.GetComponent<DialogSystem>().independentDialog("???", lines[rand]);
        }
        
    }

    public int getBidoes_Apanhados()
    {
        return bidoes_apanhados;
    }

    public void Set_Bidoes_Apanhados(int novo_num)
    {
        bidoes_apanhados = novo_num;
    }

    public void Wall_Break()
    {
        if (bidoes_apanhados == 3)
        {
            Canvas.GetComponent<DialogSystem>().ReStart(wall_break, true);
            gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog(true);
        }
    }

    public void AdvanceScene()
    {
        logSys.SaveLog();
        variable.time = time;
        FindObjectOfType<Head_Animations>().Close_Eyes_Anim("ClimbingCliff");
    }
}
