using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockElemental : MonoBehaviour
{
    bool isUnlocked;
    [SerializeField] GameObject unlockFX;
    [SerializeField] Sprite unlocked;
    [SerializeField] SpriteRenderer sr;
    public bool isFire, isLightning, isIce, isWind;

    [SerializeField] AudioClip hitSFX;
    private void Start()
    {
        if(GameManager.instance.wind && isWind ||
            GameManager.instance.fire && isFire ||
            GameManager.instance.lighning && isLightning ||
            GameManager.instance.ice && isIce)
        {
            Unlocking();
            isUnlocked = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isUnlocked && collision.CompareTag("Player"))
        {
            Unlocking();
            GameManager.instance.AddTime();
            GameManager.instance.AddTime();
            Player.instance.Shake(0.3f);
            if (hitSFX != null && AudioManager.instance != null)
                AudioManager.instance.PlaySFX(hitSFX);
        }
    }

    public void Unlocking()
    {
        sr.sprite = unlocked;
        if (isWind)
            GameManager.instance.wind = true;
        if (isFire)
            GameManager.instance.fire = true;
        if (isLightning)
            GameManager.instance.lighning = true;
        if (isIce)
            GameManager.instance.ice = true;

        isUnlocked = true;
        unlockFX.SetActive(true);
    }


}
