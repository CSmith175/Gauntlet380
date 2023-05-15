using UnityEngine;

//general interface for all entities. allows all entities to react to being shot iof they need to
public interface IGameEntity
{
    public void ReactToShot(int shotDamage, GameObject shotSourceEntity);
    public ProjectileSourceType EntityType
    {
        get;
    }
}

//interface for items in the players inventory.

public interface IInventoryItem
{
    public void UseItem();
}

//interface for enemies and spawners. Inherits from IGameEntity.
public interface IEnemy : IGameEntity
{
    public void OnDeath();
}

//interface for pickupable item in the world. Inherits from IGameEntity
public interface IPickup : IGameEntity
{
    public PickupData EntityPickupData
    {
        get;
        set;
    }

    public void PickupItem();
}

//interface for pickipable item in the world that goes into the players inventory. Inherits from IPickup
public interface IInventoryPickup : IPickup
{
    public ItemType PickupItemType
    {
        get;
        set;
    }

    public void AddItemToInventory(ItemType inventoryItem);
}
