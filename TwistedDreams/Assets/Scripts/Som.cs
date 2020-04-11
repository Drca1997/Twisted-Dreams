using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Som
{
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
    public string nome;
    [Range(0.1f, 1f)]
    public float volume;

}
