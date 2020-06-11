using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exiting_Time : MonoBehaviour
{

    public float time;
    public GameObject C;
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("MenuInicial");
        }
    }
}
