using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncefield : MonoBehaviour
{

    [SerializeField]private float bounce = 20f;
    [SerializeField] Animator anim;
    [SerializeField] AudioClip hitSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            anim.SetTrigger("Bounce");
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 0);
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);
        }

    }

}
