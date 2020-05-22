using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quente_Frio : MonoBehaviour
{

    public GameObject player;
    public Camera camera;
    private float distancia;
    private Som s;
    private Som bonk;
    private int waitfinish;
    public TextAsset final;
    public TextAsset away;
    private bool sneak_away;

    private void Awake()
    {
        sneak_away = false;
        s = FindObjectOfType<AudioManager>().getSom("Quente_Frio");
        bonk = FindObjectOfType<AudioManager>().getSom("Bonk");
    }

    // Start is called before the first frame update
    void Start()
    {

        gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog(false);
        waitfinish = 0;
    }

    // Update is called once per frame
    void Update()
    {
        distancia = Vector3.Distance(player.transform.position, camera.transform.position);

        Debug.Log("DISTANCIA DA SARAH À CAMARA: " + distancia);
        if (waitfinish <= 1) {
            Debug.Log("entrou aqui");
            calcula_quente_frio(distancia);
        }

        // if players goes against wall and no dialog is active -> depois trocar false pela cena de se bateu na parede
        if (!gameObject.GetComponentInChildren<DialogSystem>().is_active() && waitfinish == 0)
        {
            waitfinish++;
            s.source.Play();
            s.source.volume = 1;
        }

        if(!gameObject.GetComponentInChildren<DialogSystem>().is_active() && Is_Player_In_LOS() && distancia <= 2f && !gameObject.GetComponentInChildren<DialogSystem>().is_in_independent() && waitfinish == 1)
        {
            waitfinish++;
            gameObject.GetComponentInChildren<DialogSystem>().ReStart(final, false);
            gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog(false);
            Debug.Log("entrou aqui");
            s.source.volume = 0;


        }
        if (!sneak_away && !gameObject.GetComponentInChildren<DialogSystem>().is_active() && distancia >= 11f && !Is_Player_In_LOS())
        {
            sneak_away = true;
           
            gameObject.GetComponentInChildren<DialogSystem>().ReStart(away, true);
            gameObject.GetComponentInChildren<DialogSystem>().ActivateDialog(true);
            
        }

        if (gameObject.GetComponentInChildren<DialogSystem>().Is_Dialog_Finished() && waitfinish == 2)
        {
            
            //Muda para a cena seguinte
            Debug.Log("ACABOU A CENA");
            Application.Quit();
        }
    }

    public void calcula_quente_frio(float distancia)
    {
        if (distancia <= 1.2f){
            s.source.volume = 1;
        }
        else if (distancia >= 10f)
        {
            s.source.volume = 0.01f;
        }
        else
        {
            s.source.volume = 1 - distancia * 14/ 100;
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
              
                return true;
            }
        }
        return false;
    }
}
