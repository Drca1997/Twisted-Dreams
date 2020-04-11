using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quente_Frio : MonoBehaviour
{

    public GameObject player;
    public Camera camera;
    private float distancia;
    private Som s;

    private void Awake()
    {
        s = FindObjectOfType<AudioManager>().getSom("Quente_Frio");
    }
    // Start is called before the first frame update
    void Start()
    {
        s.source.Play();
        s.source.volume = 1;
    }

    // Update is called once per frame
    void Update()
    {
        distancia = Vector3.Distance(player.transform.position, camera.transform.position);
        Debug.Log("DISTANCIA DA SARAH À CAMARA: " + distancia);
        calcula_quente_frio(distancia);
    }

    public void calcula_quente_frio(float distancia)
    {
        if (distancia <= 3f){
            s.source.volume = 1;
        }
        else if (distancia >= 15f)
        {
            s.source.volume = 0.01f;
        }
        else
        {
            s.source.volume = 1 - distancia * 8/ 100;

        }
        
    }
}
