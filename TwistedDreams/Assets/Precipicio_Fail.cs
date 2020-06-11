using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class Precipicio_Fail : MonoBehaviour
{

    public GameObject car;
    public GameObject C;
    private Rigidbody rb;
    private CarController car_controller;
    private DialogSystem dialogSystem;


    private void Awake()
    {
      
        rb = car.GetComponent<Rigidbody>();
        car_controller = car.GetComponent<CarController>();
       
        dialogSystem = FindObjectOfType<DialogSystem>();
        dialogSystem.ActivateDialog(false);
    }

    private void Update()
    {
        if (Fall_Off_World())
        {
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("Tutorial");
        }
    }


    public void FixedUpdate()
    {

        if (dialogSystem.Is_Dialog_Finished())
        {
            Move_Carro();
        }

    }

    public void Move_Carro()
    {
        car_controller.Move(0f, 1f, 0f, 0f);
        //rb.AddForce(Vector3.up);

    }
    public bool Fall_Off_World()
    {

        if (car.transform.position.y <= 25f)
        {

            return true;
        }
        return false;
    }
}
