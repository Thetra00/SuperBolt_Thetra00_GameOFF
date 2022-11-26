using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] AudioClip hitSFX;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(hitSFX);

        if (collision.gameObject.CompareTag("Player"))
        {
        Player.instance.TakeDamage(transform);
        Destroy(gameObject);
        }
    }
}
