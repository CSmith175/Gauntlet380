//Used to determine if a projectile came from an enemy or a player
public enum ProjectileSourceType
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
    Empty,
    Potion,
    Key
}

//used to easily acsess a players stats in one function
public enum PlayerStatCategories
{
    MoveSpeed,
    ShotDamage,
    ShotDowntime,
    MeleeDamage,
    MagicDamage,
    Defense,
    ShotSpeed,
    Health
}
//Used for binding players to controllers
public enum PlayerNums
{
    Player1 = 1,
    Player2 = 2,
    Player3 = 3,
    Player4 = 4
}
