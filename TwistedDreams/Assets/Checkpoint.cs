using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private DrivingScene driving_script;

    private void Start()
    {
        driving_script = FindObjectOfType<DrivingScene>();
       
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger collision");
            driving_script.AdvanceCheckpoint(gameObject);
           
        }
        else
        {
            Debug.Log("NOPE");
        }
    }
    

}
