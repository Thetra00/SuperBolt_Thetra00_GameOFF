using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnder : MonoBehaviour
{
    public int[] unlockNextStageID;
    public AudioClip LESFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (LESFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(LESFX);
            LevelComplete();
        }
    }


    public void LevelComplete()
    {
        Player.instance.isActiv = false;
        Player.instance.rb.velocity = Vector2.zero;
        StartCoroutine(LevelCompleteEnd());
    }

    IEnumerator LevelCompleteEnd()
    {
        Player.instance.isActiv = false;
        GetComponent<Animator>().SetBool("Fin", true);
        Player.instance.GetComponentInChildren<Animator>().SetBool("Fin", true);
        Player.instance.rb.velocity = new Vector2( 0, Player.instance.rb.velocity.y);

        if (GameManager.instance.WinMusic != null && AudioManager.instance != null)
            AudioManager.instance.PlayMusic(GameManager.instance.WinMusic, 0f);


        yield return new WaitForSeconds(0.5f);
        Player.instance.rb.AddForce(Vector2.up * 7, ForceMode2D.Impulse);

        for (int i = 0; i < unlockNextStageID.Length; i++)
        {
            MapManager.instance.unlockedIDs.Add(unlockNextStageID[i]);
        }
        GameManager.instance.checkpoint = Vector2.zero;
        yield return new WaitForSeconds((3));
        GameManager.instance.LevelComplete();
    }

}
