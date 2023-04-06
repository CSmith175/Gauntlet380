interface IGameEntity
{
    public void ReactToShot(int shotDamage);
}

interface IInventoryItem
{
    public ItemType InventoryItemType
    {
        get;
        set;
    }

    public void UseItem();
}
interface IEnemy : IGameEntity
{
    public void OnDeath();
}

interface IPickup : IGameEntity
{
    public PickupData EntityPickupData
    {
        get;
        set;
    }

    public void PickupItem();
}

interface IInventoryPickup : IPickup
{
    public ItemType PickupItemType
    {
        get;
        set;
    }

    public void AddItemToInventory(IInventoryItem inventoryItem);
}



