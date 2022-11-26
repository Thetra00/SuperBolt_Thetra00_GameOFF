using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public int life;
    public float leveltime = 300;

    [SerializeField] Text CoinText, LifeText;
    public PowerUpType powerType;
    [SerializeField] Image PowerUpBox;
    [SerializeField] Sprite noneImg, fireImg, windImg, lighningImg, iceImg;
    [SerializeField] Image energyFill;
    [SerializeField] Image timeFill;
    public float curCD, maxCD;

    public Vector2 checkpoint;

    public GameObject[] Lifes;

    public AudioClip click;
    

    // Start is called before the first frame update
    void Start()
    {
        maxCD = GameManager.instance.maxCD;
        
    }

    // Update is called once per frame
    void Update()
    {
        EnergyFill();
        PowerState();
        LifeCount();
        CoinText.text = GameManager.instance.coins.ToString();
        timeFill.fillAmount = GameManager.instance.leveltime / 300;
    }


    public void LifeCount()
    {
        LifeText.text = GameManager.instance.life.ToString();


        //for (int i = 0; i < Lifes.Length; i++)
        //{
        //    if (i < life)
        //        Lifes[i].SetActive(true);
        //    else
        //        Lifes[i].SetActive(false);
        //}
    }

    private void PowerState()
    {
        powerType = GameManager.instance.powerType;
        switch (powerType)
        {
            case (PowerUpType.Fire):

                PowerUpBox.sprite = fireImg;
                energyFill.color = Color.red;
                break;
            case (PowerUpType.Ice):

                PowerUpBox.sprite = iceImg;
                energyFill.color = Color.blue;
                break;
            case (PowerUpType.Wind):

                PowerUpBox.sprite = windImg;
                energyFill.color = Color.magenta;
                break;
            case (PowerUpType.Lightning):

                PowerUpBox.sprite = lighningImg;
                energyFill.color = Color.yellow;
                break;
            case (PowerUpType.None):
                PowerUpBox.sprite = noneImg;
                energyFill.color = Color.gray;
                break;
        }
    }
    
    void EnergyFill()
    {
        switch (powerType)
        {
            case (PowerUpType.Fire):
                energyFill.fillAmount = GameManager.instance.curCD / maxCD;
                break;
            case (PowerUpType.Ice):
                energyFill.fillAmount = GameManager.instance.curCD / maxCD;
                break;
            case (PowerUpType.Wind):
                energyFill.fillAmount = GameManager.instance.curCD / maxCD;
                break;
            case (PowerUpType.Lightning):
                energyFill.fillAmount = GameManager.instance.curCD / maxCD;
                break;
            case (PowerUpType.None):
                energyFill.fillAmount = 0;
                break;
        }
    }


    public void BackToMap()
    {
        ScreenFader.instance.FadeON();
        Time.timeScale = 1f;
        GameManager.instance.checkpoint = Vector2.zero;
        SceneManager.LoadScene(1);
        ScreenFader.instance.FadeOFF();
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void UnPauseTime()
    {
        Time.timeScale = 1f;
    }

}
