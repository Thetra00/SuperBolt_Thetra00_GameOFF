using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Coin : Item_Pickup
{
    [SerializeField] int amount = 1;


    protected override void PickUp()
    {
        GameManager.instance.AddCoins(amount);
    }
}
