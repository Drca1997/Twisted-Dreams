using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projétil : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")){
            //se tiver tempo fazer Bounce Effect
            Debug.Log("DESAPARECEU");
            Destroy(gameObject);
          
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("BAW GAWD, SHE DED");
          
        }
            
    }
}
