using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{

    private string interact;
    public TMPro.TextMeshPro TextUI;
    private bool within_reach;
    private Som s;
    private Scene cena;
    private Revelation revelacao;
    private Tutorial tutorial;
    private GameObject player;
    private void Awake()
    {
        cena = SceneManager.GetActiveScene();
        interact = Determina_Interaction_Prompt();
        within_reach = false;
        player = GameObject.FindGameObjectWithTag("Player");
        s = FindObjectOfType<AudioManager>().getSom("Interaction_Possible");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("INTERACTION");
        if (collision.gameObject.CompareTag("Player"))
        {

            Debug.Log("INTERACTION");
            TextUI.SetText(interact);
            within_reach = true;
            s.source.Play();
           
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("END OF INTERACTION");
            TextUI.SetText("");
            within_reach = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("e") && within_reach)
        {
            Debug.Log("INTERACTION WITH OBJECT");
            
            CameraController camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
            
            if (cena.name == "Revelation")
            {
                
                revelacao = player.GetComponent<Revelation>();
                Destroy(player.GetComponent<PlayerInput>());
                revelacao.Revelacao(gameObject, revelacao.gameObject);
            }
            else if (cena.name == "Quente_Frio")
            {

            }
            else if(cena.name == "Precipício")
            {

            }
            else if (cena.name == "Tutorial")
            {
                if (gameObject.tag == "Phone")
                {
                    Destroy(TextUI);
                    Destroy(gameObject);

                    //C: "Nice Phone!"
                }
                else if(gameObject.tag == "Door")
                {
                    tutorial = GameObject.FindGameObjectWithTag("GameController").GetComponent<Tutorial>();
                    tutorial.DoorAnimation(gameObject);
                }
            }
        }
    }
    
    public string Determina_Interaction_Prompt()
    {
        if (cena.name == "Tutorial")
        {
            if (gameObject.tag == "Door")
            {
                return "  Press E\n     To\nOpen Door";
            }
            else if (gameObject.tag == "Phone")
            {
                return "   Press E\n    To Get\nYour Phone";
            }
        }
        return "E";
    }
}
