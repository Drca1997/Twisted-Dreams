using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{

    private string interact;
    public TMPro.TextMeshPro TextUI;
    [SerializeField]
    private bool within_reach;
    private Som s;
    private Scene cena;
    private Revelation revelacao;
    private Tutorial tutorial;
    private Paper paper;
    private GameObject player;
    private JohnLevel johnlevel;

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
        if (Input.GetKeyDown("e") && within_reach && !player.GetComponent<PlayerInput>().is_paused)
        {
            Debug.Log("INTERACTION WITH OBJECT");
            
            //CameraController camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
            
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
                    GameObject phone = Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<Tutorial>().hasPhone);
                    DontDestroyOnLoad(phone);
                    Debug.Log(phone);
                    if (FindObjectOfType<DialogSystem>().Is_Dialog_Finished())
                        FindObjectOfType<DialogSystem>().independentDialog("???", "Nice Phone!");
                    
                }
                else if(gameObject.tag == "Door")
                {
                    tutorial = GameObject.FindGameObjectWithTag("GameController").GetComponent<Tutorial>();
                    tutorial.DoorAnimation(gameObject);
                }
            }
            else if(cena.name == "JohnShooting")
            {
                johnlevel = GameObject.FindGameObjectWithTag("GameController").GetComponent<JohnLevel>();
                if (gameObject.tag.CompareTo("Car") == 0)
                {   
                    johnlevel.LoadNextScene("Free Driving Scene");
                }
                else if(gameObject.tag.CompareTo("Door") == 0)
                {
                    
                    if (gameObject.GetComponentInChildren<Animator>().GetBool("Aberta"))
                    {

                        DoorAnimation_Close(gameObject);
                    }
                    else
                    {

                        DoorAnimation_Open(gameObject);
                    }
                    
                }
                else if (gameObject.tag.CompareTo("JohnDoor") == 0)
                {
                    if (gameObject.GetComponentInChildren<Animator>().GetBool("Aberta"))
                    {
                       
                        DoorAnimation_Close(gameObject);
                     
                    }
                    else
                    {
                        Debug.Log("Devia Abrir");
                        DoorAnimation_Open(gameObject);
                        if (johnlevel.John.GetComponent<JohnStalking>() == null)
                        {
                            johnlevel.JohnReveal();
                        }
                    }
                }
                else if(gameObject.tag.CompareTo("Locked_Door") == 0)
                {
                    GameObject Lock = johnlevel.GetLockChild(gameObject);
                    if (Lock != null)
                    {
                        if (johnlevel.UseKey(Lock))
                        {
                            DoorAnimation_Open(gameObject);
                            gameObject.tag = "Door";
                            Destroy(Lock);      
                          
                        }
                        else
                        {
                            if (FindObjectOfType<DialogSystem>().Is_Dialog_Finished())
                                FindObjectOfType<DialogSystem>().independentDialog("???", "You still have to find the key!");
                            
                        }
                    }
                    else
                    {
                        Debug.Log("ALGO ERRADO! LOCK NULL!");
                    }
                    
                }
                else if(gameObject.tag.CompareTo("Key") == 0)
                {
                    johnlevel.PickUpKey(gameObject);
                    Destroy(TextUI);
                    Destroy(gameObject);
                }


            }
          
            else if (cena.name == "Paper")
            {
                paper = GameObject.FindGameObjectWithTag("GameController").GetComponent<Paper>();
                if (gameObject.tag == "Puzzle" && paper.Picked() == 4)
                {
                    Destroy(TextUI);
                    paper.NextScene();
                }
                if (gameObject.tag == "Lever")
                {
                    Destroy(TextUI);
                    Debug.Log("on it boss");
                    paper.SwitchCam();

                }
                else if (gameObject.tag == "Piece")
                {
                    Destroy(TextUI);
                    Debug.Log("on it boss");
                    paper.PiecePickUP();
                    Destroy(gameObject);
                }

            }
            else if (cena.name == "CamsLevel")
            {
                if (gameObject.tag.CompareTo("Door") == 0)
                {
                    if (gameObject.GetComponentInChildren<Animator>().GetBool("Aberta"))
                    {
                        
                        DoorAnimation_Close(gameObject);
                    }
                    else
                    {
                        
                        DoorAnimation_Open(gameObject);
                    }
                    
                }
                else if(gameObject.tag.CompareTo("Gas") == 0)
                {
                    Destroy(TextUI);
                    Destroy(gameObject);
                    //Contador Bidoes Apanhados
                    FindObjectOfType<CamsLevel>().Set_Bidoes_Apanhados(FindObjectOfType<CamsLevel>().getBidoes_Apanhados() + 1);
                    if (FindObjectOfType<CamsLevel>().getBidoes_Apanhados() == 3)
                    {
                        FindObjectOfType<CamsLevel>().Wall_Break();
                    }
                }
                else if(gameObject.tag.CompareTo("Car") == 0)
                {
                    if (FindObjectOfType<CamsLevel>().getBidoes_Apanhados() == 5)
                    {
                        FindObjectOfType<CamsLevel>().AdvanceScene();
                    }
                    else
                    {

                        FindObjectOfType<CamsLevel>().NEED_MORE_GAS();
                    }
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

    public void DoorAnimation_Open(GameObject porta)
    {
        //porta.GetComponentInChildren<Animator>().ResetTrigger("Fechar");
        //porta.GetComponentInChildren<Animator>().SetTrigger("Abrir");
        //porta.GetComponentInChildren<Animator>().SetBool("Aberta", true);
        porta.GetComponent<Animator>().ResetTrigger("Fechar");
        porta.GetComponent<Animator>().SetTrigger("Abrir");
        porta.GetComponent<Animator>().SetBool("Aberta", true);
        //porta.GetComponent<BoxCollider>().enabled = false;
        porta.GetComponentInChildren<cakeslice.Outline>().enabled = false;
        TextUI.SetText("");
    }

    public void DoorAnimation_Close(GameObject porta)
    {

        //porta.GetComponentInChildren<Animator>().ResetTrigger("Abrir");
        //porta.GetComponentInChildren<Animator>().SetTrigger("Fechar");
        //porta.GetComponentInChildren<Animator>().SetBool("Aberta", false);
        porta.GetComponent<Animator>().ResetTrigger("Abrir");
        porta.GetComponent<Animator>().SetTrigger("Fechar");
        porta.GetComponent<Animator>().SetBool("Aberta", false);
        porta.GetComponent<BoxCollider>().enabled = true;
        porta.GetComponentInChildren<cakeslice.Outline>().enabled = false;
        TextUI.SetText("");
    }
}
