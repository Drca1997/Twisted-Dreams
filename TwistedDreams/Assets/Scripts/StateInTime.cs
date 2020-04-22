using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public StateInTime(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
