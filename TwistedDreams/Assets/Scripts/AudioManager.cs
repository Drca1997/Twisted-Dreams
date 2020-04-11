using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  
    public Som[] sons;

    private void Awake()
    {
       
        foreach(Som s in sons)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume; 
        }
    }
 
    public void Play(string nome)
    {
        Som s = System.Array.Find(sons, som => som.nome == nome);
        if (s == null) /*Precaução caso nao exista o som pretendido*/
            return;
        s.source.Play();
    }

    public Som getSom(string nome)
    {
        Som s = System.Array.Find(sons, som => som.nome == nome);
        return s;
    }
}
