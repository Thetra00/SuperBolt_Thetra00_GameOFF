using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBot : Enemy
{

    [SerializeField] bool canStairs;
    [SerializeField] GameObject shell;

    [SerializeField] bool isFlying;
    public int startingPoint;
    public Transform[] waypoints;
    private int i;
    bool wait;
    float waittime;
    float movingSpeed;


    protected override void Start()
    {
        base.Start();
        if(isFlying)
            transform.position = waypoints[startingPoint].position;
    }

    protected override void Movement()
    {
        CheckGrounded();

        CheckWall();

        if (isStunned && cooldown > 0)
            cooldown -= Time.deltaTime;
        else
            isStunned = false;



        headOffset = transform.position.y + headpos;

        if (!isStunned)
        {

            if (!isFlying)
                Walk();
            else
                Fly();
        }
    }

    void Walk()
    {
        if (!isWall)
        {

            if (canStairs)
                rb.velocity = new Vector2(transform.localScale.x * -moveSpeed, rb.velocity.y);

            else if (!canStairs && isGrounded)
                rb.velocity = new Vector2(transform.localScale.x * -moveSpeed, rb.velocity.y);
            else
                Flip();
        }
        else
            Flip();
    }

    void Fly()
    {
        if (Vector2.Distance(transform.position, waypoints[i].position) < 0.1f)
        {
            i++;
            wait = true;
            waittime = 1;
        }

        if (i >= waypoints.Length)
        {
            i = 0;
        }

        if (!wait)
        {
            if (Vector2.Distance(transform.position, waypoints[i].position) < 0.75f)
                movingSpeed = moveSpeed * 0.5f;
            else
                movingSpeed = moveSpeed;

            transform.position = Vector2.MoveTowards(transform.position, waypoints[i].position, movingSpeed * Time.deltaTime);
            Flip();
        }
        else
        {
            waittime -= Time.deltaTime;
            if (waittime < 0)
                wait = false;
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
                    Instantiate(shell, transform.position, Quaternion.identity);
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

        if (isFlying)
        {
            Vector3 Direction = transform.position - waypoints[i].position;
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
        else
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }
    }
}
