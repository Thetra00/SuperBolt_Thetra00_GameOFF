using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Vector2 EnterDirection;
    Vector2 playerMoveDirection;
    [SerializeField] private Transform destination;
    private GameObject player;
    private bool portable;
   
    [SerializeField] private AudioClip teleportSFX;

    static bool TeleporterReady = true;


    private void Update()
    {
        playerMoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(TeleporterReady)
            if (playerMoveDirection == EnterDirection  && portable)
            {
                    StartCoroutine(Teleporting());
            }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            portable = true;
        }
     

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
            portable = false;
        
        }
    }

    IEnumerator Teleporting()
    {
        TeleporterReady = false;
        Player.instance.anim.SetTrigger("Teleport");

        yield return new WaitForSeconds(0.1f);

        if (teleportSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(teleportSFX);

    
        Player.instance.rb.velocity = Vector2.zero;

        Player.instance.transform.position = destination.position;
        player = null;
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(Portability());


    }
    IEnumerator Portability()
    {
        yield return new WaitForSeconds(0.5f);
        TeleporterReady = true;
    }

}
