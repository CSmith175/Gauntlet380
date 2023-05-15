using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Available classes a player can be, there should be 4")] private ClassData[] _classes;
    [SerializeField] [Tooltip("Maximum amount of player that can play, Goal is 4")] [Min(1)] private int _playerMax = 4;
    [SerializeField] [Tooltip("The Position that new players spawn at")] private Vector3 _playerSpawnPos = Vector3.up;

    //Current Players
    private Player[] _players;
    public Player[] Players { get { return _players; } }


    #region "Unity Functions
    //for initilization of event listeners
    private void Start()
    {
        if(_classes != null)
            EventBus.OnAvailableClassesUpdated?.Invoke(_classes);
    }


    //For Event Subscriptions
    private void OnEnable()
    {
        EventBus.OnPlayerClear.AddListener(CheckForAllPlayersClear);
        EventBus.OnPlayerDied.AddListener(CheckForAllPlayersDead);
    }
    private void OnDisable()
    {
        EventBus.OnPlayerClear.RemoveListener(CheckForAllPlayersClear);
        EventBus.OnPlayerDied.RemoveListener(CheckForAllPlayersDead);
    }
    #endregion

    #region "Controller Related"
    /// <summary>
    /// Determines the controller number (a value from 1 - 4 that represents a controller)
    /// </summary>
    /// <returns></returns>
    private int DetermineControllerNumber()
    {
        if (_players == null)
        {
            _players = new Player[_playerMax];
            return 1;
        } //null checks player array, returns one for first controller if its null

        int checkingValue = 0;

        for (int i = 0; i < _playerMax; i++)
        {
            checkingValue++;

            for (int ii = 0; ii < _players.Length; ii++)
            {
                if (_players[ii])
                {
                    if (_players[ii].ControllerNumber == checkingValue)
                        break;
                }

                if (ii == _players.Length - 1)
                    return checkingValue;

            }
        }

        Debug.LogWarning("No Controllers were available, defauting to 1");
        return 1;
    }
    #endregion

    #region "Player Class(wizard/elf, etc.) Related
    /// <summary>
    /// Selects a random class thats not in use
    /// </summary>
    /// <returns> Randomly selected class data </returns>
    private ClassData SelectRandomClassFromAvailable()
    {
        List<ClassData> classOptions = DetermineAvailiableClasses();
        if (classOptions != null) //selects a random one from available options
        {
            return classOptions[Random.Range(0, classOptions.Count)];
        }
        else if (_classes != null) //selects a random one from all that are available
        {
            return _classes[Random.Range(0, _classes.Length)];
        }

        Debug.LogError("Couldn't Select a class in SelectRandomClassFromAvailable()");
        return null;
    }
    #endregion

    #region "Player Adding/Dropping"
    /// <summary>
    /// Adds a player and feeds through inputted class data
    /// </summary>
    /// <param name="playerClass"> Class of the new player </param>
    private void AddPlayer(ClassData playerClass)
    {
        if (!playerClass)
            Debug.LogError("No playerclass found when trying to add a player");

        if (_players == null)
        {
            _players = new Player[_playerMax];
        }

        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] == null)
            {
                _players[i] = playerClass.SpawnClassPrefab().AddComponent<Player>();
                _players[i].gameObject.name = ("Player" + (i + 1) + " : " + playerClass.name);
                _players[i].InitilizePlayer(DetermineControllerNumber(), playerClass);
                _players[i].transform.position = _playerSpawnPos; //lazy way of setting to a spawn position. convert to a function later
                //updates available classes attatched to the event
                UpdateClassAvailabilityListeners();

                break;
            }
        }

        //publishes event
        EventBus.OnPlayerChanged?.Invoke(_players);
    }
    /// <summary>
    /// Drops a player from the game
    /// </summary>
    /// <param name="playerNumber"> player number to drop </param>
    private void DropPlayer(int playerNumber)
    {
        if (_players == null)
        {
            _players = new Player[_playerMax];
            return;
        } //returns if there is no array, but creates it

        playerNumber = Mathf.Clamp(playerNumber - 1, 0, _players.Length); //converts inputted int to an index and clamps

        List<Player> playerList = _players.ToList();

        RemovePlayer(playerList[playerNumber]);
        playerList.Remove(playerList[playerNumber]);


        _players = new Player[_playerMax];
        for (int i = 0; i < _players.Length; i++)
        {
            if (playerList.Count > i)
            {
                _players[i] = playerList[i];

                if (_players[i] != null && _players[i].ClassData != null) //renames the player to the appropiate player number
                    _players[i].gameObject.name = ("Player" + (i + 1) + " : " + _players[i].ClassData.name);

            }
            else
            {
                _players[i] = null;
            }
        }

        //publishes event
        EventBus.OnPlayerChanged?.Invoke(_players);
        //updates available classes attatched to the event
        UpdateClassAvailabilityListeners();
    }

    #endregion

    #region "Helper Functions"s
    /// <summary>
    /// Determines available classes
    /// </summary>
    /// <returns> List of available classes as ClassData </returns>
    private List<ClassData> DetermineAvailiableClasses()
    {
        if (_classes == null)
        {
            Debug.LogWarning("No ClassData array on PlayerManager. Could not determine available classes");
            return null;
        } //null check for class data array. warns console id there is not an array
        if (_players == null)
        {
            _players = new Player[_playerMax];
        } //null check for players array. creates a new array and continues on. 

        List<ClassData> classDataCheckingList = new List<ClassData>();
        ClassData currentClassData;

        for (int i = 0; i < _classes.Length; i++) //fills the list with all available classes
        {
            currentClassData = _classes[i];

            if (currentClassData)
            {
                classDataCheckingList.Add(currentClassData);

                for (int ii = 0; ii < _players.Length; ii++)
                {
                    if (_players[ii] && _players[ii].ClassData)
                    {
                        if (currentClassData == _players[ii].ClassData)
                        {
                            classDataCheckingList.Remove(currentClassData);
                            break;
                        }
                    }
                }
            }
        }

        if (classDataCheckingList.Count > 0)
        {
            return classDataCheckingList;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// Removes a player's character from the game, what happens in-game when a player is dropped
    /// </summary>
    private void RemovePlayer(Player player)
    {
        Destroy(player.gameObject);
    }
    /// <summary>
    /// Updates the list of availible classes through the event
    /// </summary>
    private void UpdateClassAvailabilityListeners()
    {
        //updates available classes
        List<ClassData> availableClases = DetermineAvailiableClasses();
        if (availableClases == null)
            EventBus.OnAvailableClassesUpdated?.Invoke(new ClassData[0]);
        else
            EventBus.OnAvailableClassesUpdated?.Invoke(availableClases.ToArray());
    }
    #endregion

    #region "Events"
    private void CheckForAllPlayersDead(Player[] players)
    {
        Debug.Log("Checking Players Death States ...");

        if(players != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i] != null)
                {
                    if(players[i].PlayerStats.GetPlayerStat(PlayerStatCategories.Health) > 0)
                    {
                        return;
                    }
                }
            }

            //no player had more than 0 health, invoke all players dead from event bus
            EventBus.OnAllPlayersDead?.Invoke();
        }
    }
    private void CheckForAllPlayersClear(Player[] players)
    {
        Debug.Log("Checking Players Clear States ...");

        //assumes players are set to inactive when they clear
        if (players != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].gameObject.activeSelf)
                    {
                        return;
                    }
                }
            }

            //no player was active
            EventBus.OnAllPlayersClear?.Invoke();
        }
    }
    #endregion

    //for debugging
    private void OnGUI()
    {
        //adding players debugging
        if(GUI.Button(new Rect(10 ,Screen.height - 50, 80, 20), "Add Player"))
        {
            AddPlayer(SelectRandomClassFromAvailable());
        }
        //removing players debugging
        if(_players != null && _players.Length == 4)
        {
            if (_players[0])
            {
                if(GUI.Button(new Rect(10, Screen.height - 80, 200, 20), "Drop Player 1"))
                {
                    DropPlayer(1);
                }
                if (GUI.Button(new Rect(220, Screen.height - 80, 200, 20), "Drain Player Health by 250"))
                {
                    _players[0].PlayerStats.IncrementPlayerStat(PlayerStatCategories.Health, -250);
                }
                if (GUI.Button(new Rect(430, Screen.height - 80, 200, 20), "Add to Player Score by 100"))
                {
                    _players[0].PlayerStats.IncrementPlayerStat(PlayerStatCategories.Score, 100);
                }

            }
            if (_players[1])
            {
                if (GUI.Button(new Rect(10, Screen.height - 110, 200, 20), "Drop Player 2"))
                {
                    DropPlayer(2);
                }
            }
            if (_players[2])
            {
                if (GUI.Button(new Rect(10, Screen.height - 140, 200, 20), "Drop Player 3"))
                {
                    DropPlayer(3);
                }
            }
            if (_players[3])
            {
                if (GUI.Button(new Rect(10, Screen.height - 170, 200, 20), "Drop Player 4"))
                {
                    DropPlayer(4);
                }
            }
        }


    }
}
