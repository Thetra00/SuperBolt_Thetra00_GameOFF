using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
   

    public GameObject Loot;
    [SerializeField] int lootCount;
    int i = 0;
    [SerializeField] Animator animator;
    [SerializeField] Transform SpawnPoint;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite emptyBoxImage;

    public AudioClip hitSFX;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (i < lootCount)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                i++;
                Instantiate(Loot, SpawnPoint.position, Quaternion.identity);
                animator.SetTrigger("Hit");
                if (hitSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(hitSFX);

                if (i > lootCount - 1)
                {
                    sr.sprite = emptyBoxImage;
                    Destroy(gameObject);
                }

            }
        }

    }
}
