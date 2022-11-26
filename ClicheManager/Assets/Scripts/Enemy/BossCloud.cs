using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCloud : MonoBehaviour
{
    public GameObject BossEnding;
    public GameObject BossUI;
    [SerializeField] Image BossHPBar;

    [SerializeField] int maxHP = 8;
    public int curhp;


    public bool isFighting, isDead;
    bool isRight, invul;
    float waittime;


    float movingSpeed;
    public float moveSpeed;
    public int startingPoint;
    public Transform[] waypoints;
    private int i;

    [SerializeField] Animator anim;
    [SerializeField] GameObject bossDefeatFX;
    [SerializeField] AudioClip shootSFX, hitSFX, invulSFX, EndingMusic;
    [SerializeField] Transform AttackPoint;
    [SerializeField] float timeBetweenThrow = 1.5f;
    [SerializeField] GameObject throwingWeapon, throwPickUp;
    [SerializeField] float throwHeight = 14;


    public float headpos;
    [HideInInspector] public float headOffset;

    bool  wait, hit;

    float actionTime, invultime;
    [SerializeField] float timeBetweenActions = 4;


    void Start()
    {
        transform.position = waypoints[startingPoint].position;
        curhp = maxHP;
    }


    void Update()
    {
        if (!isFighting || isDead)
        {
            BossUI.SetActive(false);
            return;
        }
        else
        {
            BossUI.SetActive(true);
            BossHPBar.fillAmount = (float)curhp / maxHP;
        }
        if (hit)
            return;

        if (invultime >= 0)
        {
           
            invul = true;
            invultime -= Time.deltaTime;
        }
        else
            invul = false;

        headOffset = transform.position.y + headpos;
        anim.SetBool("Invul", invul);
        Flip();


        if (Time.time - timeBetweenActions > actionTime)
        {
            anim.SetBool("Ready", true);
            actionTime = Time.time;

            if (Random.value > 0.6f)
                StartCoroutine(Attack());

        }
        else
            Movement();

    }


    // Update is called once per frame
    void Movement()
    {
     

        if (Vector2.Distance(transform.position, waypoints[i].position) < 0.1f)
        {
            i = Random.Range(0,waypoints.Length);
            wait = true;
            waittime = 1;
        }

     

        if (!wait)
        {
            if (Vector2.Distance(transform.position, waypoints[i].position) < 0.75f)
                movingSpeed = moveSpeed * 0.5f;
            else
                movingSpeed = moveSpeed;

            transform.position = Vector2.MoveTowards(transform.position, waypoints[i].position, movingSpeed * Time.deltaTime);
            Flip();
        }
        else
        {
            waittime -= Time.deltaTime;
            if (waittime < 0)
                wait = false;
        }

    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        Vector2 direction = (Player.instance.transform.position - transform.position).normalized;
        if (Random.value > 0.4f)
        {
            GameObject first = Instantiate(throwingWeapon, AttackPoint.position, Quaternion.identity);
            first.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x * throwHeight / 2, Mathf.Abs(direction.y * throwHeight)), ForceMode2D.Impulse);
        }
        else
        {
            GameObject first = Instantiate(throwPickUp, AttackPoint.position, Quaternion.identity);
            first.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x * throwHeight / 2, Mathf.Abs(direction.y * throwHeight)), ForceMode2D.Impulse);
        }

        if (shootSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(shootSFX);

        yield return new WaitForSeconds(timeBetweenThrow);

        GameObject second = Instantiate(throwingWeapon, AttackPoint.position, Quaternion.identity);
        anim.SetTrigger("Attack");

        second.GetComponent<Rigidbody2D>().AddForce(new Vector2(-direction.x * throwHeight / 2, Mathf.Abs(direction.y * throwHeight)), ForceMode2D.Impulse);

        if (shootSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(shootSFX);
        anim.SetBool("Ready", false);
        actionTime = Time.time;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hit && !isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.instance.Shake(0.5f);

                if (collision.gameObject.transform.position.y > (headOffset) && !invul)
                {
                    HeadJump(collision.gameObject);
                    invultime = 4f;
                }
                else
                {
                    Player.instance.TakeDamage(transform);
                }
            }
        }
    }



    protected virtual void HeadJump(GameObject player)
    {
        if (!invul)
        {

            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);

            curhp--;
            anim.SetTrigger("Hit");
            StartCoroutine(GotHit());
            hit = true;
            Player.instance.jumpCount--;

            if (curhp <= 0)
                Dead();
        }
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
        Vector2 pusDir = new Vector2(Random.Range(-3, 3), 15);
        player.GetComponent<Rigidbody2D>().AddForce(pusDir, ForceMode2D.Impulse);

    }
    IEnumerator GotHit()
    {
        actionTime = Time.time;
      
       
        yield return new WaitForSeconds(1.5f);
        if (invulSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(invulSFX);
        hit = false;
    }

    public void Dead()
    {
        AudioManager.instance.StopBossMusic();
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
        isFighting = false;
       
        Instantiate(bossDefeatFX, transform.position, Quaternion.identity);
   
        AudioManager.instance.StopBossMusic();
        AudioManager.instance.PlayMusic(EndingMusic,0f);

        BossEnding.GetComponent<Boss3Finale>().BossDead();
    }
}
