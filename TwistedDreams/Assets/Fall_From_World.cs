using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_From_World : MonoBehaviour
{
    private bool IsFalling;


    private void Start()
    {
        IsFalling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("collision");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
       if (collision.gameObject.tag == "Floor")
       {
            IsFalling = true;
       } 
    }

    public bool Get_IsFalling()
    {
        return IsFalling;
    }

    public void Set_IsFalling(bool novo_estado)
    {
        IsFalling = novo_estado;
    }
}
