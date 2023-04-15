using UnityEngine;

public struct ProjectileData
{
    public GameObject projectileVisuals;
    public int projectileDamage;
    public ProjectileSource projectileSource;
}

public struct PlayerStatValues
{
    public int ShotDamage;
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