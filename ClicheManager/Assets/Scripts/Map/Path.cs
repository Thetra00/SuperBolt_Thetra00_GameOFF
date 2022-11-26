using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform[] PathWaypoints;
    public bool isOpened = false;

    public void UnlockPath()
    {
        isOpened = true;
    }



}
