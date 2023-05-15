using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLogic : MonoBehaviour
{
    public ItemType item;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            PlayerInventory pi = other.GetComponent<Player>().PlayerInventory;
            if (pi.CheckIfInventoryFull())
            {
                return;
            }
            else
            {
                pi.TryAddItem(item);
                gameObject.SetActive(false);
            }
        }
    }
}
