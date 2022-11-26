using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlayer : MonoBehaviour
{
    public static PathPlayer instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] float speed;
    public Vector3 activLevelposition;
    private List<Transform> waypoints = new List<Transform>();
    [SerializeField]bool isMoving;
    int i = 0;

    public GameObject ThrustFX;
    Animator animator;

    private void Start()
    {
        if(MapManager.instance.playerPos != Vector3.zero)
            transform.position = MapManager.instance.playerPos;
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (isMoving)
        {
            if (Vector2.Distance(transform.position, waypoints[i].position) < 0.02f)
            {
                if (i == waypoints.Count - 1)
                {
                    activLevelposition = waypoints[i].position;
                    MapManager.instance.playerPos = activLevelposition;
                    isMoving = false;
                    i = 0;
                }
                else
                {
                    i++;

                    Vector2 dir = waypoints[i].position - transform.position;
                    animator.SetFloat("DirX", dir.x);
                    animator.SetFloat("DirY", dir.y);
                    if(dir.y > 0.1)   
                        ThrustFX.GetComponent<TrailRenderer>().sortingOrder = 1;
                    else
                        ThrustFX.GetComponent<TrailRenderer>().sortingOrder = -1;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, waypoints[i].position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("DirX", 0);
            animator.SetFloat("DirY", 0);
        }

        ThrustFX.SetActive(isMoving);
    }



    public void MoveTo (Transform[] path)
    {
        if (isMoving)
            return;

        waypoints.Clear();
        for (int i = 0; i < path.Length; i++)
        {
            waypoints.Add(path[i]);
        }
        isMoving = true;
    }



    
}
