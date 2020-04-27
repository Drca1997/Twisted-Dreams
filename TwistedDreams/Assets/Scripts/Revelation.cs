using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revelation : MonoBehaviour
{
    public GameObject espelho;
    private bool step_1 = false;
    private bool step_2 = false;
    private GameObject player;
    private GameObject camera;
    private Vector3 by_side;
   
    public void Revelacao(GameObject camera, GameObject player)
    {
        step_1 = true;
        this.player = player;
        this.camera = camera;
        by_side = new Vector3(0f, 0f, 0f);
        Interactable script = camera.GetComponent<Interactable>();
        Destroy(script.TextUI);
        Destroy(script);
    }


    private void FixedUpdate()
    {
        if (step_1 && !Player_By_Camera_Side())
        {
            player.transform.RotateAround(camera.transform.position, Vector3.up, -30*Time.fixedDeltaTime);   
        }    
        if (step_2 && Mathf.Abs(camera.transform.position.z - espelho.transform.position.z) > 0.25f)
        {
           camera.transform.position += new Vector3(0f, 0f, -1f * Time.fixedDeltaTime);
        }
        
    }


    public bool Player_By_Camera_Side()
    {
        float px = player.transform.position.x;
        float pz = player.transform.position.z;
        float cx = camera.transform.position.x;
        float cz = camera.transform.position.z;
        if (Mathf.Abs(pz - cz) <= 0.409 && px - cz <= 0.336 && px -cz >= 0)
        {
            Debug.Log("AO LADO");
            return true;
        }
        return false;

    }
    public bool Player_Behind_Camera()
    {
        float px = player.transform.position.x;
        float pz = player.transform.position.z;
        float cx = camera.transform.position.x;
        float cz = camera.transform.position.z;
        if (Mathf.Abs(px - cx) < 0.5 && pz > cz)
        {
            step_1 = false;
            step_2 = true;
            return true;
        }
        else
        {
            return false;
        }
    }


    
}
