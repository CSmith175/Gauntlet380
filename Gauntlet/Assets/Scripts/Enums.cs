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
    PlayerJoined,
    PlayerDropped,
    FoodPickedUp,
    PotionPickedUp,
    PotionUsed,
    KeyPickedUp,
    KeyUsed,
    StatBoostPickedUp,
    PlayerExit,
    PlayerDamaged,
    PlayerDied,
    PlayerHealthLow,
    Ramble
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
    Stat
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
    Health,
    Score
}

//used in the UI for determining the state of each player
public enum PlayerUIDisplayState
{
    Unjoined,
    Game,
    Dead,
    LevelComplete
}
