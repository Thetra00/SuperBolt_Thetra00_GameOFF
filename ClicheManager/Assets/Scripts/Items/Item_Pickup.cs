using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pickup : MonoBehaviour
{
   [SerializeField]private Rigidbody2D _rigidbody;
   [SerializeField] private bool _spawnBounce;
   [SerializeField] private float _spawnForce;

    public GameObject player;
    public GameObject pickupFX;
    public AudioClip pickupSFX;

    private void Start()
    {
        int rng = Random.Range(-1, 2);
        if(_spawnBounce)
            _rigidbody.AddForce(new Vector2(rng, _spawnForce), ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player == null)
            {
                player = collision.gameObject;
                PickUp();

                if (pickupSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(pickupSFX);

                Instantiate(pickupFX,transform.position,Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void PickUp()
    {

    }



}
