using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBot : Enemy
{

    [SerializeField] float timeBetweenJumps;
    [SerializeField] Vector2 JumpDir;
    bool isRight;


    [SerializeField] Transform AttackPoint;
    [SerializeField] float timeBetweenThrow = 1.5f;
    [SerializeField] GameObject throwingWeapon;
    [SerializeField] float throwHeight = 7;
    bool thrown;

    int throwCounter = 5;

    protected override void Movement()
    {

        if (Vector2.Distance(Player.instance.transform.position, transform.position) > 10)
            return;

        if (!isRight && Player.instance.transform.position.x > transform.position.x || isRight && Player.instance.transform.position.x < transform.position.x)
        {
            Flip();
        }


        if (timeBetweenJumps > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            timeBetweenJumps -= Time.deltaTime;
        }
        else
        {
            if(!thrown)StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        thrown = true;
       
        rb.velocity = Vector2.zero;

        if (jumpSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(jumpSFX);

        rb.AddForce(JumpDir, ForceMode2D.Impulse);

        GameObject first = Instantiate(throwingWeapon,AttackPoint.position,Quaternion.identity);
        anim.SetTrigger("Attack");
        throwCounter--;
        first.GetComponent<Rigidbody2D>().AddForce(new Vector2(Player.instance.transform.position.x - transform.position.x, throwHeight),ForceMode2D.Impulse);
        if (shootSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(shootSFX);
        yield return new WaitForSeconds(timeBetweenThrow);
        GameObject second = Instantiate(throwingWeapon, AttackPoint.position, Quaternion.identity);
        anim.SetTrigger("Attack");
        second.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x - Player.instance.transform.position.x, throwHeight-2), ForceMode2D.Impulse);
        thrown = false;
        throwCounter--;
        if (shootSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(shootSFX);
        timeBetweenJumps = 3.5f;
        if (throwCounter <= 0)
        {
            JumpDir = new Vector2(-JumpDir.x, JumpDir.y);
            moveSpeed *= -1;
        }
    }

    protected override void AnimationUpdate()
    {
        anim.SetBool("IsGrounded",isGrounded);
        anim.SetFloat("dirX", rb.velocity.x);
    }


    void Flip()
    {
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z); 
    }


}
