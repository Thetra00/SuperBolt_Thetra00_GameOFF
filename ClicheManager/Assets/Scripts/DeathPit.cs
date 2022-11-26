using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPit : MonoBehaviour
{

    public AudioClip deadSFX;
    [SerializeField] bool isLava;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !Player.instance.isDead)
            Player.instance.Dead();

        if (collision.gameObject.CompareTag("Item") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Destroy"))
            Deconstruct(collision.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Player.instance.Dead();

        if (collision.gameObject.CompareTag("Item")|| collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Destroy"))
            Deconstruct(collision.gameObject);
    }

    void Deconstruct(GameObject obj)
    {
        if (deadSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(deadSFX);
        Destroy(obj);
    }

 
}
