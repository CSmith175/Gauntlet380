//general interface for all entities. allows all entities to react to being shot iof they need to
interface IGameEntity
{
    public void ReactToShot(int shotDamage);
}

//interface for items in the players inventory.

interface IInventoryItem
{
    public ItemType InventoryItemType
    {
        get;
        set;
    }

    public void UseItem();
}

//interface for enemies and spawners. Inherits from IGameEntity.
interface IEnemy : IGameEntity
{
    public void OnDeath();
}

//interface for pickupable item in the world. Inherits from IGameEntity
interface IPickup : IGameEntity
{
    public PickupData EntityPickupData
    {
        get;
        set;
    }

    public void PickupItem();
}

//interface for pickipable item in the world that goes into the players inventory. Inherits from IPickup
interface IInventoryPickup : IPickup
{
    public ItemType PickupItemType
    {
        get;
        set;
    }

    public void AddItemToInventory(IInventoryItem inventoryItem);
}
