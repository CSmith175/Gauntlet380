using UnityEngine;

public struct ProjectileData
{
    public int projectileDamage;
    public ProjectileSource projectileSource;
}

public struct PlayerStatValues
{
    public int moveSpeed;
    public int shotDamage;
    public int meleeDamage;
    public int magicDamage;
    public int defense;
    public int shotSpeed;
    public int health;
}

public struct NarrationInputParing
{
    public NarrationInformationType informationType;
    public string narrationString;
}