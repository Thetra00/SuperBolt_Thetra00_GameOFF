using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTilemap : MonoBehaviour
{
   public Animator animator;
   public Collider2D col;

    private void Start()
    {
        animator.enabled = false;
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            animator.enabled = true;
            col.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.enabled = false;
            col.enabled = false;
        }
    }

}
