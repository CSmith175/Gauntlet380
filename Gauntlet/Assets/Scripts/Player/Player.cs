using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IGameEntity
{
    public ProjectileSourceType EntityType
    {
        get { return ProjectileSourceType.Player; }
    }

    #region "Variables/Properties"

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
    //closest door to the player
    public DoorLogic ClosestDoor
    {
        get;
        private set;
    }

    //for simplifying narration triggers
    private NarrationTriggerController _narrationController = new NarrationTriggerController();
    public NarrationTriggerController NarrationController { get { return _narrationController; } }

    //bound gamepad
    public Gamepad _boundPad;

    #endregion

    #region "Event Actions"
    public Action<PlayerInventory> OnInventoryUpdate;
    public Action<int> OnHealthUpdate;
    public Action<int> OnScoreUpdate;
    #endregion

    //Replacment for a constructor
    public void InitilizePlayer(ClassData classData)
    {
        _classData = classData;

        //non component based initilization
        SetUpPlayerStats(classData);
        SetUpPlayerInventory(true);

        //initilizes player controls
        gameObject.TryGetComponent(out PlayerControls pControls);
        if(pControls == null)
        {
            pControls = gameObject.AddComponent<PlayerControls>();
        }
        pControls.InitilizePlayer(this);
    }

    #region "Unity Functions"

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Door")
        {
            if(other.gameObject.TryGetComponent(out DoorLogic door))
            {
                ClosestDoor = door;
            }
        }
    }

    #endregion


    #region "Helper Functions for Player Initilization


    /// <summary>
    /// Sets up the player stats class
    /// </summary>
    /// <param name="classData"> base class data to start off with </param>
    private void SetUpPlayerStats(ClassData classData)
    {
        if(_playerStats != null)
        {
            _playerStats.InitilizePlayerStats(this);
        }
        else
        {
            _playerStats = new PlayerStats(this);
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
            _playerInventory = new PlayerInventory(this);
        }
    }

    public void ReactToShot(int shotDamage, GameObject shotSourceEntity)
    {
        _playerStats.IncrementPlayerStat(PlayerStatCategories.Health, -shotDamage);
    }

    #endregion


}
