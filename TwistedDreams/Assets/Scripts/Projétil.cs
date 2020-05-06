using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projétil : MonoBehaviour
{

    [HideInInspector]
    public float bullet_speed;

    private void Start()
    {
        bullet_speed = GameObject.FindGameObjectWithTag("John").GetComponent<EnemyAI>().bullet_speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

       
        if (collision.gameObject.CompareTag("Floor")){
            //se tiver tempo fazer Bounce Effect
            Debug.Log("DESAPARECEU");
            //FindObjectOfType<TimeManager>().StopSlowMotion();
            Destroy(gameObject);
          
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("BAW GAWD, SHE DED");
          
        }
            
    }

}
