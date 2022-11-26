using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] waypoints;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, waypoints[i].position) < 0.02f)
            i++;
            if(i == waypoints.Length)
            {
            i = 0;
            }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[i].position, speed * Time.deltaTime);
    }

}
