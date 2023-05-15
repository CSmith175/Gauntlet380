public struct ProjectileData
{
    private int _projectileDamage;
    private ProjectileSourceType _projectileSource;

    /// <summary>
    /// Property to get the projectiles's damage
    /// </summary>
    public int ProjectileDamage { get { return _projectileDamage; } }
    /// <summary>
    /// Property to get the projectiles general source
    /// </summary>
    public ProjectileSourceType projectileSourceType { get { return _projectileSource; } }

    /// <summary>
    /// Sets a projectiles stored data to given values
    /// </summary>
    /// <param name="damage"> projectile damage </param>
    /// <param name="type"> projectile source in a general sense, enemy or player </param>
    public void SetProjectileData(int damage, ProjectileSourceType type)
    {
        _projectileDamage = damage;
        _projectileSource = type;
    }
}



public struct PlayerStatValues
{
    public int moveSpeed;
    public int shotDamage;
    public float ShotDowntime;
    public int meleeDamage;
    public int magicDamage;
    public int defense;
    public int shotSpeed;
    public int health;
    public int score;
}

public struct NarrationInputParing
{
    public NarrationInformationType informationType;
    public string narrationString;
}