using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [TextArea]
    public string text;

    
    [SerializeField] Animator animator;
    [SerializeField] AudioClip talkSFX;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y< transform.position.y-0.4)
        {
            Debug.Log(DialogSystem.instance.gameObject);
            DialogSystem.instance.StartText(text);
            animator.SetTrigger("Hit");
            if (talkSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(talkSFX);
        }
    }


}
