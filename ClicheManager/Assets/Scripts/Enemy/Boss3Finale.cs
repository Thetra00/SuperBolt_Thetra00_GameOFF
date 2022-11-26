using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Finale : MonoBehaviour
{
    public bool BossDefeated = false;
    bool delComp;
    [SerializeField] Animator animator;
    public GameObject DestroyFX;
   [SerializeField] GameObject[] GameEndObj;

    public void BossDead()
    {
       StartCoroutine(EndGame());
    }


    IEnumerator EndGame()
    {
     
        delComp = true;
        Player.instance.isActiv = false;
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < GameEndObj.Length; i++)
        {
            Instantiate(DestroyFX, GameEndObj[i].transform.position, Quaternion.identity);
            Destroy(GameEndObj[i]);
            yield return new WaitForSeconds(0.1f);
        }
        ScreenFader.instance.FadeON();
        animator.SetBool("Fin", true);
    }


    public void AnimEv_FaderOff()
    {
        ScreenFader.instance.FadeOFF();
    }

   

}
