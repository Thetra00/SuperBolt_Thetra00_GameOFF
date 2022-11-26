using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom3 : MonoBehaviour
{
    public BossCloud boss;
    public GameObject BossCam;
    public AudioClip bossMusic;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
           boss.isFighting = true;
            BossCam.SetActive(true);
            AudioManager.instance.PlayBossMusic(bossMusic);
        }
    }
}
