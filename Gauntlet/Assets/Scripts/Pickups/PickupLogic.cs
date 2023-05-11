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
            other.GetComponent<PlayerInventory>().TryAddItem(item);
            gameObject.SetActive(false);
        }
    }
}
