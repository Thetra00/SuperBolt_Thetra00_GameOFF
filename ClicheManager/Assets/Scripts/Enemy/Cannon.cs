using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] float range, shootCooldown = 3;
    [SerializeField] GameObject bullet, shootFX;
    bool readyToShoot;
    [SerializeField] Transform ShootingPoint;
    Vector3 vectorToTarget;

    [SerializeField] bool rotates, isRotating;
    [SerializeField] float rotationspeed, rotationModifier;
    
    void Update()
    {
        if (Vector2.Distance(Player.instance.transform.position, transform.position) < range && shootCooldown < 0)
            readyToShoot = true;

        else
        {
            readyToShoot = false;
            shootCooldown -= Time.deltaTime;
        }


       

        if (readyToShoot)
        {
            StartCoroutine(Shoot());
        }
        if (rotates)
                RotateToPlayer();

    }

    void RotateToPlayer()
    {
        
        if (isRotating) 
        {
        vectorToTarget = Player.instance.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationspeed);
        }   
    }

    IEnumerator Shoot()
    {
        shootCooldown = 4f;
        isRotating = true;
        yield return new WaitForSeconds(1.5f);
        isRotating = false;
        yield return new WaitForSeconds(0.2f);
        Instantiate(shootFX,ShootingPoint.position, transform.rotation);
        if (rotates)
        {
            GameObject bul = Instantiate(bullet, ShootingPoint.position, transform.rotation);
            bul.GetComponent<CannonBullet>().dir = vectorToTarget;
        }
        else
        {
            vectorToTarget = Player.instance.transform.position - transform.position;
            GameObject bul = Instantiate(bullet, ShootingPoint.position, Quaternion.identity);
            if (Player.instance.transform.position.x < bul.transform.position.x)
                bul.transform.localScale = new Vector3(-1, 1, 1);
            else
                bul.transform.localScale = new Vector3(1, 1, 1);
            bul.GetComponent<CannonBullet>().dir = new Vector3(vectorToTarget.x,0,0);
        }


    }

  
}
