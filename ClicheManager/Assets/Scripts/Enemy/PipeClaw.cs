using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeClaw : Enemy
{
    public int Damage;

    [SerializeField] float respawnTime;
    bool canBite;
    float lastShow;


    void Update()
    {
        if (!playerInRange())
            return;

        if (isStunned && cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            anim.speed = 0;
        }
        else
        {       
            isStunned = false;
            anim.speed = 1;
        }



        headOffset = transform.position.y + headpos;

        if (!isStunned)
        {

            if (!isDead && Time.time - respawnTime > lastShow &&  Vector2.Distance(transform.position, Player.instance.transform.position) < 10)
            {

                canBite = true;
            }


            if (canBite)
                if (Vector2.Distance(transform.position, Player.instance.transform.position) > 2f)
                {
                    Bite();
                }
        }
    }

    void Bite()
    {
        canBite = false;
        lastShow = Time.time;
        anim.SetTrigger("Show");

   
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isStunned && !isDead)
            {
                if (shootSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(shootSFX);

                collision.gameObject.GetComponent<Player>().TakeDamage(transform);
            }
        }
    }

}
