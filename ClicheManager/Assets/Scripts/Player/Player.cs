using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public static Player instance;
    private void Awake()
    {
        instance = this;
    }

    public bool isActiv;

    public Animator anim;
    int noneLayer, fireLayer, iceLayer, windLayer, lightningLayer;
    public Rigidbody2D rb;
    [SerializeField] float moveSpeed, jumpForce;
    [SerializeField] Transform feet, wall, attackpoint;
    public float directionX, directionY, walljumpdirection;
    [HideInInspector] public int jumpCount;
    [SerializeField] GameObject JumpFX;
    Vector2 WaterJump;

    float coyoteTime = 0.2f;
    float coyoteTimeCount;




    public PowerUpType powerType;
    private int fireCount;
    [SerializeField] int maxHP = 1;
    public int curhp;
    [SerializeField] float invulTime;
    float lastHitTime;


    public bool isMoving, isDead, isStunned, isSwimming;
    [SerializeField] bool isGrounded, isWall, wallJump;
    [SerializeField] LayerMask ground, jumpGroundLayer;
    bool faceRight = true;
    bool isKnockback;

    [SerializeField] CinemachineImpulseSource impuls;
    [SerializeField] float impulsStrenght;

    public GameObject PowerUpObject;
    public ParticleSystem Bubbles;
    public AudioClip HitSFX, DeadSFX, ShootSFX, JumpSFX, ColorChangeSFX;


    void Start()
    {
        
        if(GameManager.instance.checkpoint != Vector2.zero)
        {
            transform.position = GameManager.instance.checkpoint;
        }

 

        curhp = maxHP;

        noneLayer = anim.GetLayerIndex("Base");
        fireLayer = anim.GetLayerIndex("Fire");
        iceLayer = anim.GetLayerIndex("Ice");
        windLayer = anim.GetLayerIndex("Wind");
        lightningLayer = anim.GetLayerIndex("Lightning");
        powerType = GameManager.instance.powerType;
    }


    void Update()
    {
        if (!isActiv)
        {
            return;
        }
        
        PowerUpSettings();

        if ((faceRight && directionX < 0) || (!faceRight && directionX > 0))
            Flip();

        if (isGrounded)
        {
            coyoteTimeCount = coyoteTime;
        }
        else
            coyoteTimeCount -= Time.deltaTime;



        if (!isSwimming && !isDead)
        {
            CheckGrounded();
            CheckWall();
        }

        if (Bubbles != null)
        {
            if (isSwimming)
            {
                Bubbles.Play();
                if(WaterJump.x >0)
                    WaterJump = new Vector2(WaterJump.x - Time.deltaTime*10, WaterJump.y);
                if(WaterJump.y >0)
                    WaterJump = new Vector2(WaterJump.x , WaterJump.y - Time.deltaTime*10);
                if (WaterJump.x < 0)
                    WaterJump = new Vector2(WaterJump.x + Time.deltaTime * 10, WaterJump.y);
                if (WaterJump.y < 0)
                    WaterJump = new Vector2(WaterJump.x, WaterJump.y + Time.deltaTime * 10);
            }
            else
                Bubbles.Pause();

           
        }

        if (!isKnockback && !isDead)
            PlayerInput();

        GetPowerUpType();
      
        if(!isDead)
        AnimationUpdate();
    }
    private void FixedUpdate()
    {
        if (!isActiv)
            return;
        if (!isKnockback && !isDead)
            Movement();
    }
    protected virtual void Movement()
    {
        if (powerType == PowerUpType.Lightning)
        {
            if (!wallJump && !isSwimming)
                rb.velocity = new Vector2(directionX * (moveSpeed +1), rb.velocity.y);
            else if (isSwimming)
                rb.velocity = new Vector2(directionX , directionY ).normalized * (moveSpeed +1) * 0.75f + WaterJump;
        }
        else
        {
            if (!wallJump && !isSwimming)
                rb.velocity = new Vector2(directionX * moveSpeed, rb.velocity.y);
            else if (isSwimming)
                rb.velocity = new Vector2(directionX , directionY).normalized * moveSpeed * 0.75f + WaterJump;
        }
    }


    protected virtual void AnimationUpdate()
    {
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsWall", isWall);
        anim.SetFloat("SpeedX", rb.velocity.x);
        anim.SetFloat("SpeedY", rb.velocity.y);
        anim.SetInteger("JumpCount", jumpCount);
        anim.SetBool("IsSwimming", isSwimming);
    }

    protected virtual void Attack()
    {
        GameObject bullet = null;
        switch (powerType)
        {
            case (PowerUpType.Fire):
                bullet = Instantiate(PowerUpObject, attackpoint.position, Quaternion.identity);
                bullet.GetComponent<P_Bullet>().Shoot(new Vector2(transform.localScale.x, -1).normalized, rb.velocity.x);
                if (fireCount >= 2)
                {
                    GameManager.instance.curCD = 0;
                    fireCount = 0;
                }
                if (ShootSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(ShootSFX);
                break;
            case (PowerUpType.Ice):
                 bullet = Instantiate(PowerUpObject, attackpoint.position, Quaternion.identity);
                bullet.GetComponent<P_Bullet>().Shoot(new Vector2(transform.localScale.x, -1).normalized, rb.velocity.x);
                GameManager.instance.curCD = 0;
                if (ShootSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(ShootSFX);
                break;
            case (PowerUpType.Wind):
                 bullet = Instantiate(PowerUpObject, attackpoint.position, Quaternion.identity);
                bullet.GetComponent<P_Bullet>().Shoot(new Vector2(transform.localScale.x, -1).normalized, rb.velocity.x);
                GameManager.instance.curCD = 0;
                if (ShootSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(ShootSFX);
                break;
            case (PowerUpType.Lightning):
                bullet = Instantiate(PowerUpObject, attackpoint.position, Quaternion.identity);
                bullet.GetComponent<P_Bullet>().Shoot(new Vector2(transform.localScale.x, -1).normalized, rb.velocity.x);
                GameManager.instance.curCD = 0;
                if (ShootSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(ShootSFX);
                break;
            case (PowerUpType.None):
                break;
        }
      

    }

    protected virtual void Jump()
    {
        if (isDead)
            return;

        if (isSwimming)
        {
            if (JumpSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(JumpSFX);
            WaterJump = new Vector2(directionX * 4, directionY * 4);
        }

        else
        {
            if (coyoteTimeCount > 0)
            {
                if (!isSwimming) Instantiate(JumpFX, transform.position, Quaternion.identity);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                if (JumpSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(JumpSFX);
                coyoteTimeCount = 0;
            }
            else if (!isGrounded && isWall && jumpCount < 2)
            {
                rb.velocity = new Vector2(0, 0);
                wallJump = true;
                rb.AddForce(new Vector2(-walljumpdirection * jumpForce * 0.3f, jumpForce * 0.8f), ForceMode2D.Impulse);
                jumpCount++;
                StartCoroutine(JumpFromWall());
                if (JumpSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(JumpSFX);
            }
            else if (!isGrounded && jumpCount < 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpCount++;
                if (JumpSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(JumpSFX);
            }

        }


    }

    public void TakeDamage(Transform from)
    {
        if(isActiv && !isDead)
             StartCoroutine(ProcessDamage(from));
    }

    IEnumerator ProcessDamage(Transform from)
    {
        if (HitSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(HitSFX);



        isActiv = false;
        isKnockback = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2 (-(from.position.x - transform.position.x),2).normalized;
        rb.AddForce((dir)*jumpForce / 2, ForceMode2D.Impulse);
        if(powerType != PowerUpType.None)
        {
            anim.SetTrigger("Hit");
            GameManager.instance.powerType = PowerUpType.None;
        }
        else
        {
            isDead = true;
            Dead();
        }

        yield return new WaitForSeconds(0.7f);
        isKnockback = false;
        isActiv = true;

    }


    public void Dead()
    {
        isDead = true;
        anim.SetBool("IsSwimming", false);
        anim.SetBool("Dead", true);
        if (DeadSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(DeadSFX);

        if (GameManager.instance.life < 0)
        {
            GameManager.instance.powerType = PowerUpType.None;
            GameManager.instance.LevelFailed();
            
        }
        else
        {
            GameManager.instance.powerType = PowerUpType.None;
            GameManager.instance.RestartLevel();
        }
    }

    void Flip()
    {
        faceRight = !faceRight;
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
    }


    void CheckGrounded()
    {
        if (Physics2D.OverlapCircle(feet.position, 0.1f, ground))
        {
            isGrounded = true;
            jumpCount = 0;
            wallJump = false;
            isKnockback = false;
        }
        else
            isGrounded = false;
    }
    void CheckWall()
    {
        if (Physics2D.OverlapCircle(wall.position, 0.05f, jumpGroundLayer))
        {
  
            isWall = true;
        }
        else
        {
       
            isWall = false;
        }
    }


    IEnumerator JumpFromWall()
    {
        yield return new WaitForSeconds(0.1f);
        wallJump = false;
    }



    protected virtual void PlayerInput()
    {

        directionX = Input.GetAxisRaw("Horizontal");
        directionY = Input.GetAxisRaw("Vertical");

        if (Input.GetAxisRaw("Horizontal") != 0)
            walljumpdirection = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonUp("Jump") && !wallJump && !isSwimming && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            
        }

        if (CoolDown() && (Input.GetButtonDown("Fire1")|| Input.GetKeyDown(KeyCode.J)))
        {
            Attack();
            fireCount++;
        }
    }

    public void Shake(float impulsStrenght)
    {
        impuls.GenerateImpulse(impulsStrenght);
    }

    bool CoolDown()
    {
        if (GameManager.instance.curCD < GameManager.instance.maxCD)
            {    
            GameManager.instance.curCD += Time.deltaTime;
            return false;
            }
        return true;
    }

    void GetPowerUpType()
    {
        powerType = GameManager.instance.powerType;
    }

    void PowerUpSettings()
    {
        switch (powerType)
        {
            case (PowerUpType.Fire):
                anim.SetLayerWeight(noneLayer, 0);
                anim.SetLayerWeight(fireLayer, 1);
                anim.SetLayerWeight(iceLayer, 0);
                anim.SetLayerWeight(windLayer, 0);
                anim.SetLayerWeight(lightningLayer, 0);
                rb.gravityScale = 3;
                break;
            case (PowerUpType.Ice):
                anim.SetLayerWeight(noneLayer, 0);
                anim.SetLayerWeight(fireLayer, 0);
                anim.SetLayerWeight(iceLayer, 1);
                anim.SetLayerWeight(windLayer, 0);
                anim.SetLayerWeight(lightningLayer, 0);
                rb.gravityScale = 3;
                break;
            case (PowerUpType.Wind):
                anim.SetLayerWeight(noneLayer, 0);
                anim.SetLayerWeight(fireLayer, 0);
                anim.SetLayerWeight(iceLayer, 0);
                anim.SetLayerWeight(windLayer, 1);
                anim.SetLayerWeight(lightningLayer, 0);
                rb.gravityScale = 2.5f;
                break;
            case (PowerUpType.Lightning):
                anim.SetLayerWeight(noneLayer, 0);
                anim.SetLayerWeight(fireLayer, 0);
                anim.SetLayerWeight(iceLayer,0);
                anim.SetLayerWeight(windLayer, 0);
                anim.SetLayerWeight(lightningLayer, 1);
                rb.gravityScale = 3;
                break;
            case (PowerUpType.None):
                anim.SetLayerWeight(noneLayer, 1);
                anim.SetLayerWeight(fireLayer, 0);
                anim.SetLayerWeight(iceLayer, 0);
                anim.SetLayerWeight(windLayer, 0);
                anim.SetLayerWeight(lightningLayer, 0);
                rb.gravityScale = 3;
                break;
        }
    }



}
