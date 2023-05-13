using UnityEngine;
using System;

public class PlayerStats
{
    private ClassData _baseStats; //base stats of the player. Passed in through the constructor
    private PlayerStatValues _currentStats; //current stats of the player

    #region "Public Functions"
    /// <summary>
    /// Create a new playerstats, usint the class base stats as a starting point
    /// </summary>
    /// <param name="baseStats"> Stats to set the new PlayerStats to to start out </param>
    public PlayerStats(ClassData baseStats)
    {
        InitilizePlayerStats(baseStats);
    }

    /// <summary>
    /// Initilizes the base stats
    /// </summary>
    /// <param name="baseStats"> Initial stats from a class </param>
    public void InitilizePlayerStats(ClassData baseStats)
    {
        _baseStats = baseStats;

        SetStatsFromClassData(baseStats);
    }


    /// <summary>
    /// Add to a stat by the given amount
    /// </summary>
    /// <param name="statCategory"> Stat to change </param>
    /// <param name="increment"> Amount to increment by </param>
    public void IncrementPlayerStat(PlayerStatCategories statCategory, int increment)
    {
        switch (statCategory)
        {
            case PlayerStatCategories.MoveSpeed:
                _currentStats.moveSpeed += increment;
                //prevents negative values or 0
                _currentStats.moveSpeed = Mathf.Max(1, _currentStats.moveSpeed);
                break;
            case PlayerStatCategories.ShotDamage:
                _currentStats.shotDamage += increment;
                //prevents negative values or 0
                _currentStats.shotDamage = Mathf.Max(1, _currentStats.shotDamage);
                break;
            case PlayerStatCategories.MeleeDamage:
                _currentStats.meleeDamage += increment;
                //prevents negative values or 0
                _currentStats.meleeDamage = Mathf.Max(1, _currentStats.meleeDamage);
                break;
            case PlayerStatCategories.MagicDamage:
                _currentStats.magicDamage += increment;
                //prevents negative values or 0
                _currentStats.magicDamage = Mathf.Max(1, _currentStats.magicDamage);
                break;
            case PlayerStatCategories.Defense:
                _currentStats.defense += increment;
                //prevents negative values or 0
                _currentStats.defense = Mathf.Max(1, _currentStats.defense);
                break;
            case PlayerStatCategories.ShotSpeed:
                _currentStats.shotSpeed += increment;
                //prevents negative values or 0
                _currentStats.shotSpeed = Mathf.Max(1, _currentStats.shotSpeed);
                break;
            case PlayerStatCategories.Health:
                _currentStats.health += increment;
                //prevents negative values or 0
                _currentStats.health = Mathf.Max(1, _currentStats.health);
                break;
            default:
                break;
        }
    }

    public float GetPlayerStat(PlayerStatCategories statCategory)
    {
        switch (statCategory)
        {
            case PlayerStatCategories.MoveSpeed:
                return _currentStats.moveSpeed;
            case PlayerStatCategories.ShotDamage:
                return _currentStats.shotDamage;
            case PlayerStatCategories.ShotDowntime:
                return _currentStats.ShotDowntime;
            case PlayerStatCategories.MeleeDamage:
                return _currentStats.meleeDamage;
            case PlayerStatCategories.MagicDamage:
                return _currentStats.magicDamage;
            case PlayerStatCategories.Defense:
                return _currentStats.defense;
            case PlayerStatCategories.ShotSpeed:
                return _currentStats.shotSpeed;
            case PlayerStatCategories.Health:
                return _currentStats.health;
            default:
                Debug.LogError("No Stat Category of type: <b>" + Enum.GetName(typeof(PlayerStatCategories), statCategory) + "</b> Implemented in GetPlayerStat on PlayerStats.cs. Returning 10 as a default");
                return 10;
        }
    }

    #endregion

    #region "Helper Functions"
    //helper function for setting/resetting class stats to a classData's stats. Used in the constructor
    private void SetStatsFromClassData(ClassData classData)
    {
        _currentStats.moveSpeed = classData.BaseMoveSpeed;
        _currentStats.shotDamage = classData.BaseShotAttack;
        _currentStats.ShotDowntime = classData.BaseShotDowntime;
        _currentStats.meleeDamage = classData.BaseMeleeAttack;
        _currentStats.magicDamage = classData.BaseMagicAttack;
        _currentStats.defense = classData.BaseDefense;
        _currentStats.shotSpeed = classData.BaseShotSpeed;
        _currentStats.health = classData.BaseHealth;
    }
    #endregion
}