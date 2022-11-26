using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Fire : Item_Pickup
{
    protected override void PickUp()
    {
        GameManager.instance.powerType = PowerUpType.Fire;
    }
}
