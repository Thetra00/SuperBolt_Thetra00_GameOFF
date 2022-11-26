using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom3_Bumper : MonoBehaviour
{
    
      
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite bumped, unbumped;
    [SerializeField] float waitTime = 15f;
    bool isBumped = false;
    [SerializeField] Animator risingPlatforms;


    [SerializeField] AudioClip hitSFX;

    private void Start()
    {
        sr.sprite = unbumped;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBumped && collision.CompareTag("Player"))
        {
            StartCoroutine(Bump());
            Player.instance.Shake(0.3f);
            sr.sprite = bumped;
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);
            ActivatePlatform();
        }
    }

    IEnumerator Bump()
    {
        isBumped = true;
        yield return new WaitForSeconds(4);
        DeactivatePlatform();
      
        yield return new WaitForSeconds(waitTime);

        isBumped = false;
        sr.sprite = unbumped;

    }

    void ActivatePlatform()
    {
        risingPlatforms.SetBool("IsUp", true);
        
    }

    void DeactivatePlatform()
    {
        risingPlatforms.SetBool("IsUp", false);
     
    }





}
