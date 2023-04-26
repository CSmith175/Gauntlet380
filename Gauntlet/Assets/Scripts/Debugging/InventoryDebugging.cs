using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryDebugging : MonoBehaviour
{
    private int _inventorySize;

    private PlayerInventory _testInventory;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - Screen.width * 0.8f, 0, Screen.width * 0.8f, Screen.height));

        GUILayout.BeginVertical();
        if(_testInventory != null)
        {
            GUILayout.BeginHorizontal();

            //buttons
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Item: KEY"))
                _testInventory.TryAddItem(ItemType.Key);
            if (GUILayout.Button("Add Item: POTION"))
                _testInventory.TryAddItem(ItemType.Potion);
            if (GUILayout.Button("Use Item: KEY"))
                _testInventory.TryUseItem(ItemType.Key);
            if (GUILayout.Button("Use Item: POTION"))
                _testInventory.TryUseItem(ItemType.Potion);
            GUILayout.EndHorizontal();

            //inventory display
            GUILayout.BeginVertical();
            ItemType[] inventory = _testInventory.GetInventoryInformation();
            for (int i = 0; i < inventory.Length; i++)
            {
                GUILayout.Label(Enum.GetName(typeof(ItemType), inventory[i]));
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Create New Inventory"))
            {
                _testInventory = new PlayerInventory(_inventorySize);
            }

            string numString = GUILayout.TextField(_inventorySize.ToString());

            if (!int.TryParse(numString, out _inventorySize))
            {
                _inventorySize = 12;
            }

            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();

        GUILayout.EndArea();
    }
}
