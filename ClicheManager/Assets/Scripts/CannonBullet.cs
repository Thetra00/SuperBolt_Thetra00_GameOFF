using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;

    public Animator anim;
    public float lifeTime,headpos;
  
    [HideInInspector] public float headOffset;

    [SerializeField] bool Head;
    
    public int curhp;
    public Vector3 dir;
    public bool isMoving, isDead;

    public AudioClip hit, spawn, freezeSFX;
    public bool isStunned;
    [SerializeField] bool canBeFrozen = true;
    [SerializeField] GameObject IceBlock;

    void Start()
    {
        if (lifeTime > 0)
            Destroy(gameObject, lifeTime);
        else
            Destroy(gameObject, 50);
        if (Vector2.Distance(Player.instance.transform.position, transform.position) < 20)
            if (spawn != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(spawn);
    }


    private void Update()
    {
        if (!isStunned)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
            rb.bodyType = RigidbodyType2D.Static;

        headOffset = transform.position.y + headpos;


      
       
        if(!isDead && !isStunned)
            rb.velocity = (Vector2)dir.normalized * moveSpeed;
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
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
                if (!isDead)
                    Player.instance.TakeDamage(transform);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
            Dead();
    }

    protected virtual void HeadJump(GameObject player)
    {
        Dead();
        if (hit != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(hit);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8, ForceMode2D.Impulse);
        Player.instance.jumpCount--;
    }

    public void Dead()
    {
        anim.SetTrigger("Dead");
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        rb.gravityScale = 3;
        rb.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        Destroy(gameObject, 1.8f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + headpos));
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

}
