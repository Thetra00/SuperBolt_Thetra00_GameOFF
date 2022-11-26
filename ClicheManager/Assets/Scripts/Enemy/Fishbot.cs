using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishbot : Enemy
{
    float movingSpeed;
    public int startingPoint;
    public Transform[] waypoints;
    [SerializeField] Vector2 target;
    private int i;
    bool wait;
    float waittime;

    [SerializeField] bool isHomming;
    [SerializeField] float aggroRange = 6f;

    protected override void Start()
    {
        base.Start();
        transform.position = waypoints[startingPoint].position;
        target = waypoints[startingPoint].position;
    }

    protected override void Movement()
    {
        if (!playerInRange())
            return;


        if (isStunned && cooldown > 0)
            cooldown -= Time.deltaTime;
        else
            isStunned = false;



        headOffset = transform.position.y + headpos;

        if (!isStunned)
        {
            Flip();
            if (Vector2.Distance(transform.position, Player.instance.transform.position) < aggroRange)
            {
                isHomming = true;
                movingSpeed = moveSpeed* 1.2f;
                transform.position = Vector2.MoveTowards(transform.position, Player.instance.transform.position, movingSpeed * Time.deltaTime);   
            }

            else 
            {
                isHomming = false;
                if (Vector2.Distance(transform.position, target) < 0.1f)
                {
                    i++;

                    wait = true;
                    waittime = 1;

                    if (i >= waypoints.Length)
                    {
                        i = 0;

                    }
                    target = waypoints[i].position;
                }

                if (!wait)
                {
                    if (Vector2.Distance(transform.position, target) < 0.75f)
                        movingSpeed = moveSpeed * 0.5f;
                    else
                        movingSpeed = moveSpeed;

                    transform.position = Vector2.MoveTowards(transform.position, target, movingSpeed * Time.deltaTime);
                   
                }

                else
                {
                    waittime -= Time.deltaTime;
                    if (waittime < 0)
                        wait = false;
                }
            }
        }

    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isStunned && !isDead)
        {
            rb.velocity = Vector2.zero;
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.instance.Shake(0.5f);
                if (collision.gameObject.transform.position.y > (headOffset))
                {
                    HeadJump(collision.gameObject);
                }
                else
                {
                    if (!isStunned && !isDead)
                        Player.instance.TakeDamage(transform);
                }
            }
        }
    }


    void Flip()
    {
        Vector3 Direction;
        if (isHomming)
             Direction = transform.position - Player.instance.transform.position;
        else
             Direction = transform.position - waypoints[i].position;

        if (Direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }
        else if (Direction.x > 0 && transform.localScale.x < 0)
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }
    }
}
