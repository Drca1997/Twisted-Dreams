using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float time;
    private float time_temp;
    private int passo;

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
        passo = 0;
        time_temp = time;
    }


    private void Update()
    {
        //temporario, so para testar
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeTexture();
        }
        time -= Time.deltaTime;
        if (time <= 0)
        {
            passo++;
            Lets_Get_Trippy(passo);
            time = time_temp;
        }
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
}
