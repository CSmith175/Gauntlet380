using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionInventoryItem : IInventoryItem
{
    public void UseItem()
    {
        Debug.Log("Potion Used!");
    }
}
