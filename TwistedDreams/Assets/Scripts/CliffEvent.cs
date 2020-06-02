using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.SceneManagement;
public class CliffEvent : MonoBehaviour
{

    public GameObject car;
    [Tooltip("Quanto tempo o utilizador pode ficar a premir E ate que desbloqueie Final A. Se for mais que isso, ganha Final B")]
    public float timestamp;
    private float time;
    private DialogSystem dialogSystem;
    public TextAsset fail_text;
    private bool fail;
    private bool is_E_being_pressed;

    private void Awake()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        fail = false;
        is_E_being_pressed = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            is_E_being_pressed = true;
        }
        else if (Input.GetKey(KeyCode.E) && dialogSystem.Is_Dialog_Finished())
        {
            time += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            is_E_being_pressed = false;
            if (!dialogSystem.Is_Dialog_Finished())
            {
                dialogSystem.ReStart(fail_text, false);
                dialogSystem.ActivateDialog(false);
                fail = true;
            }
        }

        if (dialogSystem.Is_Dialog_Finished())
        {
            if (!fail && time > 0)
            {
                if (time < timestamp)
                {
                    SceneManager.LoadScene("Final_A");
                }
                else
                {
                    SceneManager.LoadScene("Final_B");
                }
            }
        }
    }

    public void FixedUpdate()
    {
        
        if (!is_E_being_pressed)
            Move_Carro();
    }

    public void Move_Carro()
    {
       car.GetComponent<CarController>().Move(0f, 1f, 0f, 0f);
        
    }
}
