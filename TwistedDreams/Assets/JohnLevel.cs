using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnLevel : MonoBehaviour
{

    public GameObject C;
    public GameObject John;
    public GameObject blue_key;
    public GameObject red_key;
    private List<Key> PickedUpKeys;
    //public GameObject[] SpawnZones;
    private DialogSystem dialogSystem;
    private int keys_count;
    public TextAsset final;
    public TextAsset john_finding;
    private bool final_done;
    private int waitfinish;


    // Start is called before the first frame update
    private void Awake()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        dialogSystem.ActivateDialog(false);
        
    }

    void Start()
    {
        PickedUpKeys = new List<Key>();
        keys_count = 0;
        final_done = false;
        waitfinish = 0;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (keys_count == 2 && !final_done && waitfinish == 2)
        {
            dialogSystem.ReStart(final, true);
            dialogSystem.ActivateDialog(true);
            final_done = true;
        }
        if (dialogSystem.Is_Dialog_Finished() && waitfinish == 1)
        {
            waitfinish++;
        }
        if (waitfinish == 2 && John.GetComponent<JohnStalking>() == null)
        {
            John.AddComponent<JohnStalking>();
            
        }
       
    }


    public void JohnReveal()
    {
        dialogSystem.ReStart(john_finding, true);
        dialogSystem.ActivateDialog(false);
        waitfinish++;
    }
    
    public void LoadNextScene(string next_scene)
    {
        C.GetComponent<Head_Animations>().Close_Eyes_Anim(next_scene);
    }


    public GameObject GetLockChild(GameObject door)
    {

        for (int i = 0; i < door.transform.parent.childCount; i++)
        {
            if (door.transform.parent.GetChild(i).gameObject.tag.CompareTo("Lock") == 0)
            {
                Debug.Log(door.transform.parent.GetChild(i).gameObject);
                return door.transform.parent.GetChild(i).gameObject;
            }

        }

        return null;
    }

    public void PickUpKey(GameObject key_object)
    {
        Key new_key = new Key(key_object);
        Debug.Log("CHAVE APANHADA: " + new_key.GetKeyColor());
        PickedUpKeys.Add(new_key);
    }

    public bool UseKey(GameObject Lock) //devolve true se consegue mesmo abrir a porta
    {
      Key key_found = SearchKey(GetLockColor(Lock));
     
      if (key_found != null)
      {
            PickedUpKeys.Remove(key_found);
            keys_count++;
            if (key_found.GetKeyColor() == "blue" && John.GetComponent<JohnStalking>() == null)
            {
                GameObject porta = GameObject.FindGameObjectWithTag("JohnDoor");
                porta.GetComponent<Interactable>().DoorAnimation_Open(porta);
                JohnReveal();
            }
            return true;
      }
      else
      {
        return false;
      }
    }


    public Key SearchKey(string color)
    {
        foreach (Key k in PickedUpKeys)
        {
            if (k.GetKeyColor() == color)
            {
               
                return k;
            }
        }
        return null;
    }

    public string GetLockColor(GameObject Lock)
    {
        if (Lock.GetComponent<MeshRenderer>().materials[1].color == Color.blue){
            Debug.Log("BLUE LOCK");
            return "blue";
        }
        else if(Lock.GetComponent<MeshRenderer>().materials[1].color == Color.red)
        {
            return "red";
        }
        else
        {
            return "yellow";
        }
    }
    public class Key
    {

        private string color;

        public Key(GameObject key)
        {
            if (key.GetComponent<MeshRenderer>().material.color == Color.blue)
            {
                color = "blue";
                Debug.Log("COLOR SET TO BLUE");
            }
            else if (key.GetComponent<MeshRenderer>().material.color == Color.red){
                color = "red";
                Debug.Log("COLOR SET TO RED");
            }
            else
            {
                Debug.Log("UPS, COR NAO ASSOCIADA");
            }
        }

        public string GetKeyColor()
        {
            return color;
        }

        
    }



    public int GetCurrentState()
    {
        return waitfinish;
    }


    /*
    public void InstantiateGameElements(GameObject spawn_zone)
    {
        if (!JohnInstantiated && !KeyInstantiated)
        {
            //Instantiate um dos dois, randomly
            int num = Random.Range(1, 3);
            if (num == 1)
            {
                //Instantiate John
                GameObject john = Instantiate(John, spawn_zone.transform.position, Quaternion.Euler(0f, -90f, 0f));
                InstantiatedJohn = john;
                dialogSystem.ReStart(john_finding, true);
                dialogSystem.ActivateDialog(false);
                waitfinish++;
                GameObject last_key = Instantiate(red_key, spawn_zone.transform.position + Vector3.forward, Quaternion.Euler(0f, -90f, 0f));
                last_key.tag = "Key";
                last_key.AddComponent<Interactable>();
                last_key.GetComponent<Interactable>().TextUI = spawn_zone.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshPro>();
                last_key.AddComponent<BoxCollider>();
                JohnInstantiated = true;
            }
            else if (num == 2)
            {
                //Instantiate Key;
                GameObject new_key = Instantiate(blue_key, spawn_zone.transform.position, Quaternion.Euler(0f, -90f, 0f));
                new_key.tag = "Key";
                new_key.AddComponent<Interactable>();
                new_key.GetComponent<Interactable>().TextUI = spawn_zone.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshPro>();
                new_key.AddComponent<BoxCollider>();
                KeyInstantiated = true;
            }
        }
        else if (!JohnInstantiated && KeyInstantiated)
        {
            //Instantiate John
            GameObject john = Instantiate(John, spawn_zone.transform.position, Quaternion.Euler(0f, -90f, 0f));
            InstantiatedJohn = john;
            dialogSystem.ReStart(john_finding, true);
            dialogSystem.ActivateDialog(false);
            waitfinish++;
            GameObject last_key = Instantiate(red_key, spawn_zone.transform.position + Vector3.forward, Quaternion.Euler(0f, -90f, 0f));
            last_key.tag = "Key";
            last_key.AddComponent<Interactable>();
            last_key.GetComponent<Interactable>().TextUI = spawn_zone.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshPro>();
            last_key.AddComponent<BoxCollider>();
            JohnInstantiated = true;
        }
        else if (JohnInstantiated && !KeyInstantiated)
        {
            //Instantiate Key;
            GameObject new_key = Instantiate(blue_key, spawn_zone.transform.position, Quaternion.Euler(0f, -90f, 0f));
            new_key.tag = "Key";
            new_key.AddComponent<Interactable>();
            new_key.GetComponent<Interactable>().TextUI = spawn_zone.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshPro>();
            new_key.AddComponent<BoxCollider>();
            KeyInstantiated = true;
        }
    }
    
    public void Check_Spawn(string door_name)
    {
        if (door_name.CompareTo("LeftDoor") == 0)
        {
            if (SpawnZones[0] != null)
            {
                InstantiateGameElements(SpawnZones[0]);
                SpawnZones[0] = null;
            } 
        }
        else if(door_name.CompareTo("RightDoor") == 0){
            if (SpawnZones[1] != null)
            {
                InstantiateGameElements(SpawnZones[1]);
                SpawnZones[1] = null;
            }
        }

    }
    */

}
