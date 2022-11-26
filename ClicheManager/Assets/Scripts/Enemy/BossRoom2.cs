using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom2 : MonoBehaviour
{
    public GameObject Boss;
    public GameObject BossEntrance, Cam;
    public GameObject[] spawnObj;
    public Transform[] SpawnPos;
    [SerializeField] float actionTime, timeBetweenActions = 4;
    bool isFighting;
    
    private void Update()
    {
        if(isFighting)
            if (Time.time - timeBetweenActions > actionTime)
            {
                actionTime = Time.time;
            
            Instantiate(spawnObj[Random.Range(0, spawnObj.Length)], SpawnPos[Random.Range(0, SpawnPos.Length)].position, Quaternion.identity);
           
            }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFighting = true;
            Cam.SetActive(true);
            BossEntrance.SetActive(true);
            Boss.GetComponent<BossFish>().isFighting = true;
            AudioManager.instance.PlayBossMusic(Boss.GetComponent<BossFish>().bossMusic);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isFighting = false;
        Destroy(gameObject, 0.1f);
        if (Boss != null)
            Boss.GetComponent<BossFish>().isFighting = false;
        AudioManager.instance.StopBossMusic();
    }
}
