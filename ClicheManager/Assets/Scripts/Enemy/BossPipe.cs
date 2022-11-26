using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPipe : MonoBehaviour
{
    public GameObject BossExit;
    public GameObject BossUI;
    [SerializeField] Image BossHPBar;
    [SerializeField] GameObject bossDefeatFX;
    [SerializeField] ParticleSystem steam;
    public AudioClip bossMusic, attackSFX, spawnSFX, deadSFX;
    [SerializeField] Animator anim;
    [SerializeField] Transform[] spawnPositions;
    Transform lastSpawn;

    [SerializeField] GameObject[] bossThrowObj;
    [SerializeField] Transform throwPos;
    [SerializeField] float throwHeight;

    [SerializeField] int maxHP = 1;
    public int curhp;
    bool despawn;

    public bool isFighting, isOut, isDead;
    [SerializeField] float actionTime, timeBetweenActions = 3;

    void Start()
    {
        isFighting = false;
    }

  
    void Update()
    {
        if (!isFighting)
        {
            BossUI.SetActive(false);
            return;
        }
        else
            BossUI.SetActive(true);

        if (Time.time - timeBetweenActions > actionTime)
        {
            actionTime = Time.time;

            if (isOut)
            {
                if (despawn)
                {
                    Despawn();
                    despawn = false;
                }

                if (Random.value > 0.3)
                {
                        Throw();
                  
                }
                else
                    Despawn();
            }
            else
            {
                Spawn();
            }
        }

        BossHPBar.fillAmount = (float)curhp / maxHP;
    }


    void Throw()
    {
        if (transform.position.x < Player.instance.transform.position.x)
            anim.SetTrigger("Left");
        else
            anim.SetTrigger("Right");

        if (attackSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(attackSFX);
        GameObject obj = Instantiate(bossThrowObj[Random.Range(0, bossThrowObj.Length)], throwPos.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Player.instance.transform.position.x - transform.position.x, throwHeight), ForceMode2D.Impulse);
        obj = Instantiate(bossThrowObj[Random.Range(0, bossThrowObj.Length)], throwPos.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2((Player.instance.transform.position.x - transform.position.x) * 0.75f, throwHeight * 1.5f), ForceMode2D.Impulse);
    }

    
    void Despawn()
    {
        isOut = false;
               
        anim.SetBool("IsOut", false);
    }

    void Spawn()
    {
        steam.Play();
        if (spawnSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(spawnSFX);
        actionTime = Time.time;
        isOut = true;
        int rand = Random.Range(0, spawnPositions.Length);
        if (lastSpawn = spawnPositions[rand])
        {
            rand = Random.Range(0, spawnPositions.Length);
            lastSpawn = spawnPositions[rand];
        }
        else
            lastSpawn = spawnPositions[rand];

        transform.position = lastSpawn.position;
        anim.SetBool("IsOut", true);
      
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

        if (curhp <= 0)
            Dead();

        despawn = true;
    }

    public void Dead()
    {
        AudioManager.instance.StopBossMusic();
        GetComponent<Collider2D>().enabled = false;
        Player.instance.jumpCount--;
        isDead = true;
        isFighting = false;
        if (deadSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(deadSFX);
        Instantiate(bossDefeatFX, BossExit.transform.position, Quaternion.identity);
        Instantiate(bossDefeatFX, transform.position, Quaternion.identity);
        Destroy(BossExit, 1f);

        AudioManager.instance.StopBossMusic();
        Destroy(gameObject, 1.8f);
    }

}
