using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Player"))
            {
            collision.GetComponent<Player>().TakeDamage(transform);
            }
        if(collision.CompareTag("Enemy"))
            {
                if (collision.GetComponent<Enemy>() != null)
                    collision.GetComponent<Enemy>().TakeDamage(1);
                if (collision.GetComponent<BossFish>() != null)
                    collision.GetComponent<BossFish>().TakeDamage(1);

            }
    }
}
