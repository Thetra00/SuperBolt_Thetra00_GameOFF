using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public List<int> unlockedIDs = new List<int>();
    public List<int> finishedIDs = new List<int>();


    public Vector3 playerPos;




   
}
