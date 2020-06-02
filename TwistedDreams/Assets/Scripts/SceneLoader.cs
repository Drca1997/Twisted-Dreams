using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Scene cena;
    private string next_scene_name;
    public GameObject SarahCamera;
    private Head_Animations head_animations;
    private void Start()
    {
        cena = SceneManager.GetActiveScene();
        head_animations = SarahCamera.GetComponent<Head_Animations>();
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            next_scene_name = Determina_Nova_Cena();
            if (head_animations)
            {
                Debug.Log("HEAD ANIMATIONS");
                head_animations.Close_Eyes_Anim(next_scene_name);
            }
            else
            {
                Debug.Log("HEAD ANIMATIONS NULL");
            }
          
           
        }
    }

    private string Determina_Nova_Cena()
    {
        string scene_name = null;
        if (cena.name == "Tutorial")
        {
            scene_name = "Quente_Frio";
        }
        else if (cena.name == "Quente_Frio")
        {
            scene_name = "JukeboxScene";
        }
        else if (cena.name == "ClimbingCliff")
        {
            scene_name = "Precipício";
        }
        return scene_name;
    }

    
}
