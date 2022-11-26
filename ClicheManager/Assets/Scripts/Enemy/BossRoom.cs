using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{


    public GameObject Boss;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Boss.GetComponent<BossPipe>().isFighting = true;
            AudioManager.instance.PlayBossMusic(Boss.GetComponent<BossPipe>().bossMusic);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        Destroy(gameObject,0.1f);
        if(Boss != null)
            Boss.GetComponent<BossPipe>().isFighting = false;
        AudioManager.instance.StopBossMusic();
    }
}
