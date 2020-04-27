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
    }

    private void FixedUpdate()
    {
        if (step_1 && !Player_Behind_Camera())
        {
            player.transform.RotateAround(camera.transform.position, Vector3.up, -30*Time.fixedDeltaTime);
        }    
        if (step_2 && Mathf.Abs(camera.transform.position.z - espelho.transform.position.z) > 0.25f)
        {
           camera.transform.position += new Vector3(0f, 0f, -1f * Time.fixedDeltaTime);
        }
        
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
