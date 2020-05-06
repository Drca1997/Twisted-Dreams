using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownRewinding = 0.5f;
    private GameObject player;
    private PlayerInput player_stats;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>().player;
        player_stats = player.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Time.timeScale += (slowdownFactor) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }
    public void SlowMotion()
    {
         
        
         Time.timeScale = slowdownFactor;
      




            Time.fixedDeltaTime = Time.timeScale * 0.02f;


    }

    public void StopSlowMotion()
    {
        Time.timeScale = 1f;
       
    }


   

}
