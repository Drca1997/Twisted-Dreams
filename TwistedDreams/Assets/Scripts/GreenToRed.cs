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
    private bool started_walking;


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
       
        actual_waypoint = -1;
        rb = C.GetComponent<Rigidbody>();
        dialogSystem = FindObjectOfType<DialogSystem>();
        parado = new Vector3 (0, 0, 0);
        dialogSystem.ActivateDialog(false);
        wait_finish = 0;
        is_moving = false;
        started_walking = false;
    }


    private void Update()
    {
        
        if (is_moving)
        {
            MoveCamera();
        }
        /*if (dialogSystem.Is_Dialog_Finished() && wait_finish < 8)
        {
            MoveCamera();
            
        }
       */
        

        if (dialogSystem.Is_Dialog_Finished() && rb.velocity == parado)
        {

            actual_waypoint++;
            passo = actual_waypoint;
            Lets_Get_Trippy(passo);
            wait_finish++;
            is_moving = true;
            if (actual_waypoint > 0)
            {
                dialogSystem.ReStart(guioes[actual_waypoint - 1], true);
                dialogSystem.ActivateDialog(true);
            }
            
        }
        

        
    }


    public void MoveCamera()
    {
   
        Vector3 dir = (WayPoints[actual_waypoint].transform.position - C.transform.position).normalized * camera_speed;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = 
            Quaternion.Slerp(GameObject.FindGameObjectWithTag("MainCamera").transform.rotation, lookRotation, Time.deltaTime * 3f);
        rb.velocity = dir;

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
