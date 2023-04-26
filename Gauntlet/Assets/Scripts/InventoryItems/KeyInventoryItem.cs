using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInventoryItem : IInventoryItem
{
    public void UseItem()
    {
        Debug.Log("Key Used!");
    }
}
