using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float basespeed;
    float pulseSpeed;
    public Vector2 direction;
    [SerializeField] int damage;
    [SerializeField] GameObject hitFX;

    int bounces;
    public AudioClip hitSFX;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    public void Shoot(Vector2 direction, float speed)
    {
        if (speed < basespeed)
            pulseSpeed = basespeed;
        else
            pulseSpeed = speed;
        rb.AddForce(new Vector2(direction.x * (pulseSpeed), direction.y), ForceMode2D.Impulse);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounces++;
       
        if(bounces > 3)
            Destroy(gameObject,0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);

            Instantiate(hitFX, transform.position, Quaternion.identity);
            HitEnemy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Destroy"))
        {
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);

            if (collision.gameObject.GetComponent<BreakableBox>() != null)
                collision.gameObject.GetComponent<BreakableBox>().DestroyBrick();

            if (collision.gameObject.GetComponent<Shellbot_Shell>() != null)
                collision.gameObject.GetComponent<Shellbot_Shell>().DestroyShell();

            if (collision.gameObject.GetComponent<CannonBullet>() != null)
                {
                Instantiate(hitFX, transform.position, Quaternion.identity);
                HitEnemy(collision.gameObject);
                }

            Instantiate(hitFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    protected virtual void HitEnemy(GameObject obj)
    {
        if (obj.GetComponent<Enemy>() != null)
        {
            obj.GetComponent<Enemy>().TakeDamage(damage);
        }
        if(obj.GetComponent<BossPipe>() != null)
            obj.GetComponent<BossPipe>().TakeDamage(1);
        Instantiate(hitFX, transform.position, Quaternion.identity);
       
        Destroy(gameObject);
    }

}
