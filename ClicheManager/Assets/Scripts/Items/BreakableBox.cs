using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    [SerializeField] GameObject Brick;
    public GameObject breakFX;
    [SerializeField] AudioClip breakSFX;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        
            DestroyBrick();
        }
    }

    public void DestroyBrick()
    {
        if (breakSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(breakSFX);

        Destroy(Brick, 0.05f);
        Instantiate(breakFX, transform.position+new Vector3(0f, 0.5f,0), Quaternion.identity);
    }

}
