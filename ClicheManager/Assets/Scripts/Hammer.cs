using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public AudioClip HitSFX;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
         {
            Player.instance.TakeDamage(transform);
            if (HitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(HitSFX);
            Destroy(gameObject);
        }
    }

}
