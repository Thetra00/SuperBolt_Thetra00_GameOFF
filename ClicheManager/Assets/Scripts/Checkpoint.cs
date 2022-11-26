using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public AudioClip CheckpointSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckpointReached();
        
        }
    }

    public void CheckpointReached()
    {

        if (GameManager.instance.checkpoint != (Vector2)transform.position)
        {
            GameManager.instance.AddTime();
            
            if (CheckpointSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(CheckpointSFX);

            GameManager.instance.checkpoint = transform.position;
        }
        GetComponent<Animator>().SetBool("Check", true);
    }
}
