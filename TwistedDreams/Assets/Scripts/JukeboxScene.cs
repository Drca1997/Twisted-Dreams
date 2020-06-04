using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeboxScene : MonoBehaviour
{
    public float time;
    public GameObject C;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
       
        if (time <= 0)
        {
            C.GetComponent<Head_Animations>().Close_Eyes_Anim("Tutorial");
        }
    }
}
