using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{ 
    [Range(0.5f, 2f)]
    [SerializeField] float fallDelay = 1f;
    [Range(1f, 5f)]
    [SerializeField] float destroyDelay = 2f;
    [Range(2f, 15f)]
    [SerializeField] float respawnTime = 10f;
    Vector2 startPos;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;

    public AudioClip fallSFX;
    bool falling;

    Transform player;

    private void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPos = transform.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!falling && collision.gameObject.transform.position.y > transform.position.y)
                StartCoroutine(Fall());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            player.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.SetParent(null);
        }
       
    }


    IEnumerator Fall()
    {
        falling = true;
        anim.SetTrigger("Step");
        yield return new WaitForSeconds(fallDelay);
        if (fallSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(fallSFX);

        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyDelay);
        StartCoroutine(Resapwn());

        player.SetParent(null);
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sr.Length; i++)
            sr[i].enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.rotation = 0;
    }

    IEnumerator Resapwn()
    {
        yield return new WaitForSeconds(respawnTime);      
        transform.position = startPos;
        transform.rotation = Quaternion.identity;

        GetComponent<Collider2D>().enabled = true;
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sr.Length; i++)
            sr[i].enabled = true;

        falling = false;

    }


  



}
