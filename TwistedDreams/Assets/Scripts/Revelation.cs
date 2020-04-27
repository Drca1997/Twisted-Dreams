using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revelation : MonoBehaviour
{
    public GameObject espelho;
    public GameObject characters;
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
        characters.GetComponent<Animator>().enabled = true;
        characters.GetComponent<Animator>().SetTrigger("Trigger");
        
    }  
}
