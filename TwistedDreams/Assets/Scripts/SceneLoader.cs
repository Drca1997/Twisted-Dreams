using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Scene cena;
    private string next_scene_name;

    private void Start()
    {
        cena = SceneManager.GetActiveScene();
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            next_scene_name = Determina_Nova_Cena();
            SceneManager.LoadScene(next_scene_name);
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
        return scene_name;
    }
}
