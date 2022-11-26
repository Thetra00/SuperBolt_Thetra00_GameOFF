using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketBall : MonoBehaviour
{
    int score;
    [SerializeField] Text scoreBoard;
    [SerializeField] AudioClip scoreSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            score++;
            if ( AudioManager.instance != null)
                AudioManager.instance.PlaySFX(scoreSFX);
            scoreBoard.text = score.ToString();
        }
    }
    
}
