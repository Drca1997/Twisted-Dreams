using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.ImageEffects;


public class GreenToRed : MonoBehaviour
{
    private TerrainLayer terreno;
    public Texture2D nova_textura;
    private Texture2D antiga_textura;
    private VignetteAndChromaticAberration VignetteAndChromaticAberration;
    private Bloom Bloom;
    private SunShafts SunShafts;
    private GlobalFog GlobalFog;
    private ColorCorrectionCurves ColorCorrectionCurves;
 
    public GameObject C;
    public GameObject[] WayPoints;
    [SerializeField]
    private int actual_waypoint;
    private Rigidbody rb;
    private DialogSystem dialogSystem;
    private Vector3 parado;
    public float camera_speed;
    [SerializeField]
    private int passo;
    [SerializeField]
    private int wait_finish;
    private bool is_moving;
    public TextAsset[] guioes;
    [SerializeField]
    private bool started_walking;
    private float time;
    private NavMeshAgent agent;
    private Transform target;


    private void Awake()
    {
        GameObject camara = GameObject.FindGameObjectWithTag("MainCamera");
        terreno = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Terrain>().terrainData.terrainLayers[0];
        antiga_textura = terreno.diffuseTexture;
        VignetteAndChromaticAberration = camara.GetComponent<VignetteAndChromaticAberration>();
        Bloom = camara.GetComponent<Bloom>();
        SunShafts = camara.GetComponent<SunShafts>();
        GlobalFog = camara.GetComponent<GlobalFog>();
        ColorCorrectionCurves = camara.GetComponent<ColorCorrectionCurves>();
        agent = C.GetComponent<NavMeshAgent>();
        actual_waypoint = 0;
        rb = C.GetComponent<Rigidbody>();
        dialogSystem = FindObjectOfType<DialogSystem>();
        parado = new Vector3 (0, 0, 0);
        dialogSystem.ActivateDialog(false);
        wait_finish = 0;
        is_moving = false;
        time = 1f;
        started_walking = false;
    }


    private void Update()
    {
        
       if (dialogSystem.Is_Dialog_Finished())
        {
            MoveCamera();
            FaceTarget();
            
            if (agent.velocity == parado && !started_walking)
            {
                started_walking = true;
            }
            else if (agent.velocity == parado && started_walking)
            {
                started_walking = false;
                actual_waypoint++;
                passo++;
                Lets_Get_Trippy(passo);
                dialogSystem.ReStart(guioes[actual_waypoint - 1], true);
                dialogSystem.ActivateDialog(true);
            }

       }
      
      
        
    }



    public void MoveCamera()
    {
        target = WayPoints[actual_waypoint].transform;
        agent.SetDestination(target.position);
        FaceTarget();
        /*Vector3 dir = (WayPoints[actual_waypoint].transform.position - C.transform.position).normalized * camera_speed;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = 
            Quaternion.Slerp(GameObject.FindGameObjectWithTag("MainCamera").transform.rotation, lookRotation, Time.deltaTime * 3f);
        rb.velocity = dir;
        Debug.Log(rb.velocity);
        float distance = Vector3.Distance(WayPoints[actual_waypoint].transform.position, C.transform.position);
        Debug.Log(distance);
        */
    }

    void FaceTarget()
    {
        
        Vector3 direction = (target.position - C.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        C.transform.rotation = Quaternion.Slerp(C.transform.rotation, lookRotation, Time.deltaTime * 3f);
    }

    public void ChangeTexture()
    {
        if (terreno.diffuseTexture == antiga_textura)
        {
            terreno.diffuseTexture = nova_textura;
        }
        else
        {
            terreno.diffuseTexture = antiga_textura;
        }
    }

    public void Lets_Get_Trippy(int passo)
    {

        if (passo == 1) {
            Bloom.enabled = true;
            SunShafts.enabled = true;
        }
        else if(passo == 2)
        {
            GlobalFog.enabled = true;
            //Instantiate John
        }
        else if (passo == 3)
        {
            ColorCorrectionCurves.enabled = true;
            VignetteAndChromaticAberration.enabled = true;
        }
        else if (passo == 4)
        {
            //ultimo passo
            ChangeTexture();
            
        }
        

      
    }

    public void LoadNextScene(string next_scene)
    {
        C.GetComponent<Head_Animations>().Close_Eyes_Anim(next_scene);
    }



}
