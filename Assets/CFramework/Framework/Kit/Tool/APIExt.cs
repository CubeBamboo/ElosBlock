using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class APIExt
{
    public static GameObject Instantiate(this GameObject go)
    {
        return Object.Instantiate(go);
    }

    public static GameObject SetPosition(this GameObject go, Vector3 pos)
    {
        go.transform.position = pos;
        return go;
    }

    public static GameObject SetRotation(this GameObject go, Quaternion rot)
    {
        go.transform.rotation = rot;
        return go;
    }
}
