using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownRewinding = 0.5f;


    private void Update()
    {
        Time.timeScale += (slowdownFactor) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }
    public void SlowMotion()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Rewindable>().isRewinding)
        {
            Time.timeScale = slowdownFactor;
           
        }
        else
        {
            Time.timeScale = slowdownFactor;
           
        }
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;


    }

    public void StopSlowMotion()
    {
        Time.timeScale = 1f;
    }


   

}
