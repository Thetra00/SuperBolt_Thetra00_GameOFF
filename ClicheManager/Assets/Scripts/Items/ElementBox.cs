using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBox : MonoBehaviour
{
    public bool isFire, isLightning, isWind, isIce;
    bool looted;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite emptyBoxImage, fullBoxImage;
    public GameObject Loot;

    [SerializeField] BoxCollider2D ground, trigger;

    [SerializeField] Animator animator;
    [SerializeField] Transform SpawnPoint;

    public AudioClip hitSFX;


    void Update()
    {
        if (!looted)
        {
            if ((isFire && GameManager.instance.fire)
                || (isLightning && GameManager.instance.lighning)
                || (isWind && GameManager.instance.wind)
                || (isIce && GameManager.instance.ice))
            {
                ground.enabled = trigger.enabled = true;
                sr.sprite = fullBoxImage;
            }
            else
            {
                ground.enabled = trigger.enabled = false;
            }
        }
    }

   


    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (!looted && collision.gameObject.CompareTag("Player"))
       {
        looted = true;
        if (hitSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(hitSFX);

            Instantiate(Loot, SpawnPoint.position, Quaternion.identity);
        animator.SetTrigger("Hit");
        sr.sprite = emptyBoxImage;
        Destroy(gameObject);
       }
    }
}
