using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Ice : P_Bullet
{
    protected override void HitEnemy(GameObject obj)
    {
        if(obj.GetComponent<Enemy>() != null)
            obj.GetComponent<Enemy>().Frozen();
        if(obj.GetComponent<CannonBullet>() != null)
            obj.GetComponent<CannonBullet>().Frozen();
        base.HitEnemy(obj);
    }
}
