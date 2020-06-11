using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Path_Taken", menuName = "Path_Taken")]
public class Path_Taken : ScriptableObject
{
    public int path; //0 - indica não definido; 1 - indica john->driving->cams path; 2 - indica john->prisao path
}
