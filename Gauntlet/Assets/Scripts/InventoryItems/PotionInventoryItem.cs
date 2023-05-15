using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionInventoryItem : IInventoryItem
{
    //set on the input in PlayerControls
    static public int _potionDamage;

    public void UseItem()
    {
        ApplyPotionDamage();
        Debug.Log("Potion Used!");
    }

    private void ApplyPotionDamage()
    {
        //stuff here with _potionDamage
    }
}
