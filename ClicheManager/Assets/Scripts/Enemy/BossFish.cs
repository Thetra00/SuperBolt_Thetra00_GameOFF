using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFish : MonoBehaviour
{

    public GameObject BossExit, BossEntrance;
    public GameObject BossUI;
    [SerializeField] Image BossHPBar;

    [SerializeField] GameObject bossDefeatFX;
    public AudioClip bossMusic, attackSFX, deadSFX;

    [SerializeField] Animator anim;
    [SerializeField] Vector2 target;

    [SerializeField] float moveSpeed, attackSpeed;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] int maxHP = 1;
    public int curhp;
    bool attack;

    public bool isFighting, isDead;
    [SerializeField] float actionTime, timeBetweenActions = 4;






    private void Update()
    {
        if (!isFighting ||  isDead)
        {
            BossUI.SetActive(false);
            return;
        }
        else
            BossUI.SetActive(true);

        Flip();

        if (Time.time - timeBetweenActions > actionTime)
        {
          
            actionTime = Time.time;

            if (Random.value > 0.6f)
                StartCoroutine(Attack());
           
        }

        if (!attack)
            rb.velocity = (Player.instance.transform.position - transform.position).normalized * moveSpeed;

        BossHPBar.fillAmount = (float)curhp / maxHP;

    }

    IEnumerator Attack()
    {
        attack = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("Attack", true);

        if (attackSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(attackSFX);

        target = (Player.instance.transform.position - transform.position).normalized;

        yield return new WaitForSeconds(0.5f);
        rb.velocity = target * attackSpeed;
        rb.AddForce((target) * attackSpeed, ForceMode2D.Impulse); //Attack

        yield return new WaitForSeconds(3);
        rb.velocity = Vector2.zero;
        attack = false;
        actionTime = Time.time;

        anim.SetBool("Attack", false);
     
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.instance.TakeDamage(transform);
            }
        }

    }


    public void TakeDamage(int Damage)
    {

        anim.SetTrigger("Hit");
        curhp -= Damage;


        if (deadSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(deadSFX);

        if (curhp <= 0)
            Dead();

    
    }

    public void Dead()
    {
        AudioManager.instance.StopBossMusic();
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
        isFighting = false;
        Instantiate(bossDefeatFX, BossExit.transform.position, Quaternion.identity);
        Instantiate(bossDefeatFX, BossEntrance.transform.position, Quaternion.identity);
        Instantiate(bossDefeatFX, transform.position, Quaternion.identity);
        Destroy(BossEntrance, 1f);
        Destroy(BossExit, 1f);

        AudioManager.instance.StopBossMusic();
        Destroy(gameObject, 1.8f);
    }


    void Flip()
    {
        Vector3 Direction = transform.position - Player.instance.transform.position;
        if (Direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }
        else if (Direction.x > 0 && transform.localScale.x < 0)
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }
    }



}
