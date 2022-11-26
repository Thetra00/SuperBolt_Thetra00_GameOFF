using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum PowerUpType { None, Fire, Ice, Wind, Lightning }


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public int life;
    public int coins;
    
    public float leveltime = 300;
    public static int activLevelID;

    bool timerjingel;

    public AudioClip timerSFX,lifeSFX, FailSFX, WinMusic;

    public bool fire, wind, lighning, ice;
    public GameObject F_Bullet, I_Bullet, W_Bullet, L_Bullet;
    


    //------------------------ UI SYSTEM --------------------- //
    public PowerUpType powerType;

    public float curCD, maxCD;

    public Vector2 checkpoint;

    private void Update()
    {
        PowerState();
        LevelTime();

        if (life <= 0)
        {
            LevelFailed();
        }
    }

    public void AddLife(int amount)
    {
        if (lifeSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(lifeSFX);
        life += amount;
        if(life > 9)
            life = 9;
    }


    public void AddCoins(int amount)
    {
        coins += amount;
        if (coins > 99)
        {
            coins -= 100;
            AddLife(1);
        }
    }

    public void UnlockFire()
    {
        fire = true;
    }

    public void UnlockWind()
    {
        wind = true;
    }

    public void UnlockLightning()
    {
        lighning = true;
    }

    public void UnlockIce()
    {
        ice = true;
    }

    private void PowerState()
    {
        if (Player.instance != null)
        {
            switch (powerType)
            {
                case (PowerUpType.Fire):
                    Player.instance.PowerUpObject = F_Bullet;

                    break;
                case (PowerUpType.Ice):
                    Player.instance.PowerUpObject = I_Bullet;

                    break;
                case (PowerUpType.Wind):
                    Player.instance.PowerUpObject = W_Bullet;

                    break;
                case (PowerUpType.Lightning):
                    Player.instance.PowerUpObject = L_Bullet;

                    break;
                case (PowerUpType.None):
                    Player.instance.PowerUpObject = null;

                    break;
            }
        }
    }


    public void RestartLevel()
    {
        if (lifeSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(FailSFX);
        StartCoroutine(RestartLevelCO());
    }


    public void LevelFailed()
    {
        Player.instance.isActiv = false;
        

        if (FailSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(FailSFX);

        checkpoint = Vector2.zero;
        ScreenFader.instance.FadeON();
        StartCoroutine(BackToMap()); //Map ID
        GameManager.instance.life = 3;
    }

    public void LevelComplete()
    {
        Player.instance.isActiv = false;

        //WIN AUDIO
        MapManager.instance.finishedIDs.Add(activLevelID);
        ScreenFader.instance.FadeON();
        StartCoroutine(BackToMap());
    }
    IEnumerator RestartLevelCO()
    {
        if (FailSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(FailSFX);

        leveltime = 300;

        if (life <= 0)
        {
            LevelFailed();
            yield return null;
        }
        else
        {
            life--;
            Player.instance.isActiv = false;
            ScreenFader.instance.FadeON();
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator BackToMap()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    public void BackToCred()
    {
        StartCoroutine(BackToCredits());
    }
    IEnumerator BackToCredits()
    {
        ScreenFader.instance.FadeON();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }


    public void AddTime()
    {
    if (lifeSFX != null && AudioManager.instance != null)
        AudioManager.instance.PlaySFX(lifeSFX);
    leveltime += 60;
    }

    void LevelTime()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            leveltime = 300;
            timerjingel = false;
            return;
        }


        if (leveltime > 300)
            leveltime = 300;

        leveltime -= Time.deltaTime;

        if(!timerjingel && leveltime < 46)
        {
            timerjingel = true;
            AudioManager.instance.PlaySFX(timerSFX);
        }


        if (leveltime < 0)
        {
            Player.instance.Dead();
            leveltime = 300;
            timerjingel = false;
        }
    }




}
