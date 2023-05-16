using System.Collections.Generic;
using System.Linq;

public class PlayerInventory
{
    private readonly Player _attatchedPlayer;

    private readonly int _maxItemAmount;
    private ItemType[] _inventoryItems;

    //classes using the item interface that holds functions for use when an item is used
    private Dictionary<ItemType, IInventoryItem> _inventoryItemDictionary;

    #region "Constructors"
    /// <summary>
    /// Constructor that creates an empty inventory with the default size of 12. Constructors with parameters exsist for specifying initial inventory state and size as well
    /// </summary>
    public PlayerInventory(Player player)
    {
        _attatchedPlayer = player;

        _maxItemAmount = 12;
        _inventoryItems = new ItemType[_maxItemAmount];

        IntilizeInventoryItemDictionary();
    }

    /// <summary>
    /// Constructor that allows the inventory's size to be set
    /// </summary>
    /// <param name="maxItemAmount"> Maximum capacity of the inventory. </param>
    public PlayerInventory (Player player, int maxItemAmount)
    {
        _attatchedPlayer = player;

        _maxItemAmount = maxItemAmount;
        _inventoryItems = new ItemType[maxItemAmount];

        IntilizeInventoryItemDictionary();
    }

    /// <summary>
    /// Constructor that allows an initial inventory to be set along with a size
    /// </summary>
    /// <param name="initialInventory"> Initial Inventory Items for the inventory </param>
    /// <param name="maxItemAmount"> Maximum capacity of the inventory. (Will clamp the initial inventory if its larger) </param>
    public PlayerInventory(Player player, ItemType[] initialInventory, int maxItemAmount)
    {
        _attatchedPlayer = player;

        _maxItemAmount = maxItemAmount;
        _inventoryItems = new ItemType[maxItemAmount];

        for (int i = 0; i < maxItemAmount; i++)
        {
            _inventoryItems[i] = initialInventory[i];
        }

        FormatInventory();

        IntilizeInventoryItemDictionary();
    }
    #endregion

    #region "Public Functions"
    /// <summary>
    /// Adds item of given Itemtype to the inventory, assuming there is space
    /// </summary>
    /// <param name="newItem"> Item Type to Add </param>
    public void TryAddItem(ItemType newItem)
    {
        int addIndex = FindFirstEmptySlot();

        if(addIndex < _inventoryItems.Length)
        {
            _inventoryItems[addIndex] = newItem;
            FormatInventory();
        }
    }

    /// <summary>
    /// Uses item of given type, assuming one is present. does nothing if the player dosn't have any of the item.
    /// </summary>
    /// <param name="type"> Item Type to Use </param>
    public bool TryUseItem(ItemType type)
    {
        if (_inventoryItems != null)
        {
            for (int i = 0; i < _inventoryItems.Length; i++)
            {
                if (_inventoryItems[i] == ItemType.Empty)
                {
                    //inventory is always moved to the left so when a null is gotten there are no more items
                    return false;
                }

                if (_inventoryItems[i] == type)
                {
                    //an item of the type has been found

                    //uses item
                    if(_inventoryItemDictionary.TryGetValue(type, out IInventoryItem item))
                    {
                        item.UseItem();
                    }

                    //emptys item slot and formats inventory
                    _inventoryItems[i] = ItemType.Empty;
                    FormatInventory();
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if an item of a given type is present in the player's inventory
    /// </summary>
    /// <param name="type"> Type to check for </param>
    /// <returns></returns>
    public bool CheckForItemOfType(ItemType type)
    {
        if(_inventoryItems != null)
        {
            for (int i = 0; i < _inventoryItems.Length; i++)
            {
                if (_inventoryItems[i] == ItemType.Empty)
                {
                    //inventory is always moved to the left so when an empty is gotten there are no more items
                    return false;
                }

                if(_inventoryItems[i] == type)
                {
                    //an item of the type has been found
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a copy of the player's inventory array
    /// </summary>
    /// <returns></returns>
    public ItemType[] GetInventoryInformation()
    {
        if(_inventoryItems != null)
        {
            return _inventoryItems;
        }
        else
        {
            _inventoryItems = new ItemType[12];
            return _inventoryItems;
        }
    }

    /// <summary>
    /// Checks if the inventory is full
    /// </summary>
    /// <returns> true if full, false if not full</returns>
    public bool CheckIfInventoryFull()
    {
        if(FindFirstEmptySlot() == _inventoryItems.Length)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets all items in the inventory to empty
    /// </summary>
    public void EmptyInventory()
    {
        if(_inventoryItems != null)
        {
            for (int i = 0; i < _inventoryItems.Length; i++)
            {
                _inventoryItems[i] = ItemType.Empty;
            }
        }
    }
    #endregion

    #region "Helper Functions"
    //initilizes the classes used to trigger item use functions
    private void IntilizeInventoryItemDictionary()
    {
        _inventoryItemDictionary = new Dictionary<ItemType, IInventoryItem>();

        _inventoryItemDictionary.Add(ItemType.Key, new KeyInventoryItem());
        _inventoryItemDictionary.Add(ItemType.Potion, new PotionInventoryItem());
    }

    /// <summary>
    /// Returns the index of the first available empty slot in the inventory. If there is none returns the length of the inventory
    /// </summary>
    /// <returns></returns>
    private int FindFirstEmptySlot()
    {
        if(_inventoryItems != null)
        {
            for (int i = 0; i < _inventoryItems.Length; i++)
            {
                if(_inventoryItems[i] == ItemType.Empty)
                {
                    return i;
                }
            }
        }

        return _inventoryItems.Length;
    }

    //Helper function that moves the inventory into the first available slots and sorts it by item type
    private void FormatInventory()
    {
        if(_inventoryItems != null)
        {
            List<ItemType> inventoryList = _inventoryItems.ToList();
            inventoryList.Sort(InventorySortFunction);

            _inventoryItems = inventoryList.ToArray();

            //fires Update event. Done here because format inventory is called after basicaly any inventory change
            if(_attatchedPlayer)
                _attatchedPlayer.OnInventoryUpdate?.Invoke(this);
        }
    }

    //function used to sort the inventory by it's type
    private int InventorySortFunction(ItemType a, ItemType b)
    {
        if((int)a < (int)b)
        {
            return 1;
        }
        else if ((int)a > (int)b)
        {
            return -1;
        }
        return 0;
    }
    #endregion
}