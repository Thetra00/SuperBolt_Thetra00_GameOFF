using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivBarrel : MonoBehaviour
{
    [SerializeField] GameObject exploFX;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") 
            || collision.gameObject.CompareTag("Player") 
            || collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(exploFX,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
