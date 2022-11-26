using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shroobot : Enemy
{
    [SerializeField] bool canStairs;
    protected override void Movement()
    {

        CheckGrounded();

        CheckWall();

        if (!isWall)
        {
            
            if(canStairs)
                rb.velocity = new Vector2(transform.localScale.x * -moveSpeed, rb.velocity.y);

            else if(!canStairs && isGrounded)
                rb.velocity = new Vector2(transform.localScale.x * -moveSpeed, rb.velocity.y);
            else
                Flip();
        }
        else
            Flip();
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
    }

}
