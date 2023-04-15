//Used to determine if a projectile came from an enemy or a player
public enum ProjectileSource
{
    Player,
    Enemy
}

//used to switch the cameras mode depending on player count throguh a delegate
public enum CameraModes
{
    SinglePlayer,
    Multiplayer
}

//used to determine which pool of narrations to pull from
public enum NarrationType
{
    Undefined,
    Damage,
    Treasure,
    Key,
    Potion,
    Food,
    LowHealth,
}

//used to help organize data for narration
public enum NarrationInformationType
{
    NoType,
    PlayerClassName,
    EnemyName,
    FoodName,
    DamageValue,
    HealValue,
    TreasureValue,
}

//used to determine which inventoryItem a player has picked up
public enum ItemType
{
    Potion,
    Key
}

//used to easily acsess a players stats in one function
public enum PlayerStats
{
    MoveSpeed,
    ShotDamage,
    MeleeDamage,
    MagicDamage,
    Defense,
    ShotSpeed,
    Health
}
