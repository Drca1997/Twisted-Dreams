using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamsLevel : MonoBehaviour
{
    private int bidoes_apanhados;

    private void Awake()
    {
        bidoes_apanhados = 0;
    }

    public int getBidoes_Apanhados()
    {
        return bidoes_apanhados;
    }

    public void Set_Bidoes_Apanhados(int novo_num)
    {
        bidoes_apanhados = novo_num;
    }
}
