using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IShootingEntity
{
    public ProjectileSourceType EntityType
    {
        get { return ProjectileSourceType.Player; }
    }



    //used controller bindings
    private int _controllerNumber;
    public int ControllerNumber
    {
        get { return _controllerNumber; }
    }



    //Class data Instance, (Elf, Mage, Warrior, etc.) used in a few places
    private ClassData _classData;
    public ClassData ClassData
    {
        get { return _classData; }
    }
    //Player Stats Class Instance, tracks the current stats of the player such as health, speed, etc. 
    private PlayerStats _playerStats;
    public PlayerStats PlayerStats
    {
        get
        {
            return _playerStats;
        }
    }
    //Player Inventory Instance, tracks the players current inventory of potions and keys
    private PlayerInventory _playerInventory;
    public PlayerInventory PlayerInventory
    {
        get
        {
            return _playerInventory;
        }
    }

    public void InitilizePlayer(int controllerNumber, ClassData classData)
    {
        //non component based initilization
        SetUpPlayerStats(classData);
        SetUpPlayerInventory(true);

        //component based initilization

        bool playerControlsInitilized = false;
        _classData = classData;


        foreach(var component in gameObject.GetComponents(typeof(Component)))
        {
            if (component is PlayerControls)
            {
                SetUpPlayerMovement(component as PlayerControls, controllerNumber);
                playerControlsInitilized = true;
            }
        }

        if(!playerControlsInitilized)
        {
            SetUpPlayerMovement(null, controllerNumber);
        }
    }



    #region "Helper Functions for Player Initilization

    /// <summary>
    /// Sets up Player Controls monobehavior. Resets one if its passed in, otherwise attatches a new one
    /// </summary>
    /// <param name="controls"> pass in a controls here to reset it instead of creating a new one </param>
    private void SetUpPlayerMovement(PlayerControls controls, int controllerNumber)
    {
        _controllerNumber = controllerNumber; //sets the held controller number

        if (controls) //resets a currently exsisting controls
        {
            controls.InitilizePlayer(controllerNumber, this);
        }
        else //creates a new controls and sets it up
        {
            gameObject.AddComponent<PlayerControls>().InitilizePlayer(controllerNumber, this);

        }
    }

    /// <summary>
    /// Sets up the player stats class
    /// </summary>
    /// <param name="classData"> base class data to start off with </param>
    private void SetUpPlayerStats(ClassData classData)
    {
        if(_playerStats != null)
        {
            _playerStats.InitilizePlayerStats(classData);
        }
        else
        {
            _playerStats = new PlayerStats(classData);
        }
    }
    /// <summary>
    /// Sets up the Player Inventory
    /// </summary>
    /// <param name="clearInventory"> If there is an inventory already, (such as from a previous player) clear it? </param>
    private void SetUpPlayerInventory(bool clearInventory)
    {
        if(_playerInventory != null)
        {
            if(clearInventory)
            {
                //empties the inventory
                _playerInventory.EmptyInventory();
            }
        }
        else
        {
            _playerInventory = new PlayerInventory();
        }
    }

    public void ReactToShot(int shotDamage)
    {
        _playerStats.IncrementPlayerStat(PlayerStatCategories.Health, -shotDamage);
    }

    #endregion
}
