using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCrash : MonoBehaviour
{
    private Som crash_sound;
    // Start is called before the first frame update
    void Start()
    {
        crash_sound = FindObjectOfType<AudioManager>().getSom("Car_Crash");
    }


    private void OnCollisionEnter(Collision collision)
    {
        crash_sound.source.Play();   
    }

}
