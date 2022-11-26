using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shellbot_Shell : MonoBehaviour
{
    [SerializeField] Transform wall1;
    [SerializeField] LayerMask ground;



    [SerializeField] Rigidbody2D rb;
    [Range(5f, 20f)]
    [SerializeField] float kickforce = 15;
    [SerializeField] GameObject breakFX;
    Vector2 dir;

    public AudioClip hitSFX;

    bool isRight = true;

    private void Update()
    {
        if(Wallcheck())
            Flip();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
       

        if (collision.gameObject.CompareTag("Player"))
        {
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);

            if (rb.velocity.magnitude < 0.5f || collision.transform.position.y > transform.position.y+0.3f)
            {
                kickforce = 15;
                Player.instance.rb.velocity = Vector2.zero;
                Player.instance.rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                Instantiate(breakFX, transform.position, Quaternion.identity);
                if ((isRight && transform.position.x > collision.transform.position.x) || (!isRight && transform.position.x < collision.transform.position.x))
                {
                    if(isRight)
                        rb.velocity = new Vector2(kickforce,0);
                    else
                        rb.velocity = new Vector2(-kickforce, 0);
                }
                else
                    Flip();
            }
            else
            {
                Instantiate(breakFX, transform.position, Quaternion.identity);

                Player.instance.TakeDamage(transform);
                Flip();
            } 
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);

            collision.gameObject.GetComponent<Enemy>().rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            Instantiate(breakFX, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            
        }

        if (collision.gameObject.CompareTag("Destroy"))
        {
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);

            Instantiate(breakFX, transform.position, Quaternion.identity);
            if (collision.gameObject.GetComponent<BreakableBox>() != null)
                collision.gameObject.GetComponent<BreakableBox>().DestroyBrick();

            if (collision.gameObject.GetComponent<Shellbot_Shell>() != null)
                collision.gameObject.GetComponent<Shellbot_Shell>().DestroyShell();
            Flip();
        }
    }

    public void DestroyShell()
    {
        Instantiate(breakFX, transform.position, Quaternion.identity);

        Destroy(gameObject, 0.05f);
    }

    void Flip()
    {
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        kickforce *= 0.75f;

        if (isRight)
            rb.velocity = new Vector2(kickforce, 0);
        else
            rb.velocity = new Vector2(-kickforce, 0);
    }

    bool Wallcheck()
    {
        if (Physics2D.OverlapCircle(wall1.position, 0.2f, ground))
        {
            return true;
        }
        else
            return false;

    }

}
