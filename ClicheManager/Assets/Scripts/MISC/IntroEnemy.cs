using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEnemy : MonoBehaviour
{
    //Move
    public float speed = 4f;

    public Rigidbody2D rb;

    public Transform feetPos, wallPos;
    public LayerMask ground;

    public AudioClip dead, hit;


    // Update is called once per frame
    void Update()
    {

        if (!isWall())
            rb.velocity = new Vector2(transform.localScale.x * speed, rb.velocity.y);
        else
            Flip();

    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
    }

    
    bool isWall()
    {
        if (Physics2D.OverlapCircle(wallPos.position, 0.1f, ground))
        {
            return true;
        }
        else
            return false;
    }

   

}
