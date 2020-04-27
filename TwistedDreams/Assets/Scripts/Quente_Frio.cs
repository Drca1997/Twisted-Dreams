using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quente_Frio : MonoBehaviour
{

    public GameObject player;
    public Camera camera;
    private int times_against_wall;
    private float distancia;
    private Som s;
    private Som bonk;
    public TextAsset bonk1;
    public TextAsset bonk2;

    private void Awake()
    {
        s = FindObjectOfType<AudioManager>().getSom("Quente_Frio");
        bonk = FindObjectOfType<AudioManager>().getSom("Bonk");
    }
    // Start is called before the first frame update
    void Start()
    {
        bonk.source.Play();
        s.source.Play();
        s.source.volume = 1;

        gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog();
        times_against_wall = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        distancia = Vector3.Distance(player.transform.position, camera.transform.position);
        //Debug.Log("DISTANCIA DA SARAH À CAMARA: " + distancia);
        calcula_quente_frio(distancia);

        // if players goes against wall and no dialog is active -> depois trocar false pela cena de se bateu na parede
        if (!gameObject.GetComponentInChildren<DialogSystem>().is_active() && false)
        {
            times_against_wall++;
            if (times_against_wall <= 1)
            {
                gameObject.GetComponentInChildren<DialogSystem>().ReStart(bonk1);
                gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog();
            }
            else
            {
                gameObject.GetComponentInChildren<DialogSystem>().ReStart(bonk2);
                gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog();
            }
        }

        Debug.Log(Is_Player_In_LOS());
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

    public bool Is_Player_In_LOS()
    {
        var RaycastDirection = player.transform.position - camera.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, RaycastDirection, out hit))
        {
            if (hit.transform == player.transform)
            {
                Debug.Log("I SPEAK THE TRUUU MA FRIEND");
                return true;
            }
            
        }
        return false;
    }
}
