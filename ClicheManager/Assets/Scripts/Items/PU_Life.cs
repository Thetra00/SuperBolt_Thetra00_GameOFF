using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Life : Item_Pickup
{
    protected override void PickUp()
    {
        GameManager.instance.AddLife(1);
    }
}
