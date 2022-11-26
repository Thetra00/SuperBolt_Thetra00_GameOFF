using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_FireBall : MonoBehaviour
{
    [SerializeField] AudioClip fireBallSFX;
    [SerializeField] Transform StartPos;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpPower;
    [SerializeField] Animator anim;
    float jumped = 0f;
    [SerializeField] float timeBetweenJumps = 2.5f;
    [SerializeField] bool delayed;
    SpriteRenderer sr;
    private IEnumerator Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (delayed)
        {
            yield return new WaitForSeconds(2);
            delayed = false;
        }
        else
            yield return null;
    }


    private void Update()
    {
        anim.SetFloat("DirY", rb.velocity.y);

        if (delayed)
            transform.position = StartPos.position;

        else
        {
            
            if (transform.position.y < StartPos.position.y)
            {
                sr.enabled = false;
                if (jumped < 0)
                {
                    jumped = timeBetweenJumps;
                    PlayAudio();
                    transform.position = StartPos.position + new Vector3(0,0.01f, 0);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                }
                else
                {
                    jumped -= Time.deltaTime;

                }
            }
            else
                sr.enabled = true;

        }

      

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (transform.position.y > StartPos.position.y)
        if (collision.gameObject.CompareTag("Player"))
        {
            if (fireBallSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(fireBallSFX);
            Player.instance.TakeDamage(transform);  
        }
    }

    void PlayAudio()
    {
        if (Vector2.Distance(Player.instance.transform.position, StartPos.position) < 20f)
             if (fireBallSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(fireBallSFX);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(StartPos.position, new Vector2(StartPos.position.x, StartPos.position.y + (jumpPower * 0.65f)));
    }

}
