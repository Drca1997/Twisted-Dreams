using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class CliffEvent : MonoBehaviour
{

    public GameObject car;

    public void FixedUpdate()
    {
        
        Move_Carro();
    }

    public void Move_Carro()
    {
       car.GetComponent<CarController>().Move(0f, 1f, 0f, 0f);
        
    }
}
