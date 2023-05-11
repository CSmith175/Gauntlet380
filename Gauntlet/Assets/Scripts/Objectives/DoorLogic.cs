using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OpenDoor(PlayerInventory playerInventory)
    {
        if (playerInventory.CheckForItemOfType(ItemType.Key))
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            playerInventory.TryUseItem(ItemType.Key);
        }
    }
}
