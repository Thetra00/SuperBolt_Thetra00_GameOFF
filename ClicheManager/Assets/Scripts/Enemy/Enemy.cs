using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public float moveSpeed;
    [SerializeField] Transform feet, wall;
    public float headpos;
    [HideInInspector]public float headOffset;
    float playerDetectionRange = 20f;
  
    [SerializeField] int maxHP = 1;
    public int curhp;

    public bool isMoving, isDead, isStunned;
    [HideInInspector] public float cooldown;
    public bool isGrounded, isWall;
    [SerializeField] LayerMask ground;

    public AudioClip hitSFX, shootSFX, jumpSFX, freezeSFX;

    [SerializeField] bool canBeFrozen = true;
    [SerializeField] GameObject IceBlock;

    public bool playerInRange()
    {
        if (Vector2.Distance(Player.instance.transform.position, transform.position) < playerDetectionRange)
            return true;
        else
             return false;
    }



    protected virtual void Start()
    {
        curhp = maxHP;
    }





    protected virtual void FixedUpdate()
    {
        if (!playerInRange())
            return;

        headOffset = transform.position.y + headpos;

        if (!isStunned && !isDead)
            Movement();

        if (!isStunned)
        {
            AnimationUpdate();
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
            rb.bodyType = RigidbodyType2D.Static;
    }


    protected virtual void Movement()
    {


    }

    protected virtual void AnimationUpdate()
    {


    }

    public void CheckGrounded()
    {
        if (Physics2D.OverlapCircle(feet.position, 0.1f, ground))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }
    public void CheckWall()
    {
        if (Physics2D.OverlapCircle(wall.position, 0.1f, ground))
        {
            isWall = true;
        }
        else
            isWall = false;
    }



    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isStunned && !isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.instance.Shake(0.5f);
                if (collision.gameObject.transform.position.y > (headOffset))
                {
                    HeadJump(collision.gameObject);
                }
                else
                {
                    Player.instance.TakeDamage(transform);
                }
            }
        }
    }

    protected virtual void HeadJump(GameObject player)
    {
        if (hitSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(hitSFX);
      

        TakeDamage(1);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8, ForceMode2D.Impulse);
       
    }

    public void TakeDamage(int Damage)
    {
        anim.SetTrigger("Hit");
        curhp -= Damage;
        if (curhp <= 0)
            Dead();
    }

    public void Dead()
    {
        rb.gravityScale = 3;
        GetComponent<Collider2D>().enabled = false;
        Player.instance.jumpCount--;
        isDead = true;
        anim.SetTrigger("Dead");
        Destroy(gameObject, 1.8f);
    }

  


    public void Frozen()
    {

        if (!canBeFrozen)
            return;
        if (freezeSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(freezeSFX);
      
        isStunned = true;
        IceBlock.SetActive(true);

        StartCoroutine(Defroze());
    }

    IEnumerator Defroze()
    {
        yield return new WaitForSeconds(4);
        Instantiate(IceBlock.GetComponent<IceBlock>().IceEffekt, transform.position, Quaternion.identity);
        IceBlock.SetActive(false);
        isStunned = false;
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + headpos));
       
    }




}
