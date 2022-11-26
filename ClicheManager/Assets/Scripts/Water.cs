using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] AudioClip WEnter, WExit;
    [SerializeField] Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().isSwimming = true;
            anim.SetBool("Water", true);
            if (WEnter != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(WEnter);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Water", false);
            collision.GetComponent<Player>().isSwimming = false;
            if (WExit != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(WExit);
        }
    }


}
