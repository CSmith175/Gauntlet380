using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Available classes a player can be, there should be 4")] private ClassData[] _classes;
    [SerializeField] [Tooltip("Maximum amount of player that can play, Goal is 4")] [Min(1)] private int _playerMax = 4;
    [SerializeField] [Tooltip("The Position that new players spawn at")] private Vector3 _playerSpawnPos = Vector3.up;
    public ClassData[] Classes
    {
        get { return _classes; }
    }
    private Player[] _players;

    //camera related
    private CameraController _cameraController;
    [Space(10)]
    [SerializeField] [Tooltip("Scriptable Object Settings for the Camera")] private CameraSettings _cameraSettings;

    private void Awake()
    {
        //attatches controller and saves refrence for updating display modes
        _cameraController = gameObject.AddComponent<CameraController>();
        if(_cameraSettings)
        {
            _cameraController.IntilizeCameraController(_cameraSettings); //applies the settings to the camera
        }
    }

    private void AddPlayer(ClassData playerClass)
    {
        if (!playerClass)
            Debug.LogError("No playerclass found when trying to add a player");

        if(_players == null)
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
                break;
            }
        }

        //updates camera
        _cameraController.UpdateCamera(_players);
    }

    private int DetermineControllerNumber()
    {
        if(_players == null)
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
                if(_players[ii])
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

    private ClassData SelectRandomClassFromAvailable()
    {
        List<ClassData> classOptions = DetermineAvailiableClasses();
        if (classOptions != null) //selects a random one from available options
        {
            return classOptions[Random.Range(0, classOptions.Count)]; 
        }
        else if(_classes != null) //selects a random one from all that are available
        {
            return _classes[Random.Range(0, _classes.Length)];
        }

        Debug.LogError("Couldn't Select a class in SelectRandomClassFromAvailable()");
        return null;
    }

    private List<ClassData> DetermineAvailiableClasses() // returns a list of the available classes (the classes that aren't currently in use). returns null if they are all in use
    {
        if(_classes == null)
        {
            Debug.LogWarning("No ClassData array on PlayerManager. Could not determine available classes");
            return null;
        } //null check for class data array. warns console id there is not an array
        if(_players == null)
        {
            _players = new Player[_playerMax];
        } //null check for players array. creates a new array and continues on. 

        List<ClassData> classDataCheckingList = new List<ClassData>();
        ClassData currentClassData;

        for (int i = 0; i < _classes.Length; i++) //fills the list with all available classes
        {
            currentClassData = _classes[i];

            if(currentClassData)
            {
                classDataCheckingList.Add(currentClassData);

                for (int ii = 0; ii < _players.Length; ii++)
                {
                    if(_players[ii])
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

        if(classDataCheckingList.Count > 0)
        {
            return classDataCheckingList;
        }
        else
        {
            return null;
        }
    }

    private void DropPlayer(int playerNumber)
    {
        if(_players == null)
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
            if(playerList.Count > i)
            {
                _players[i] = playerList[i];

                if(_players[i]) //renames the player to the appropiate player number
                    _players[i].name = _players[i].gameObject.name = ("Player" + (i + 1) + " : " + _players[i].ClassData.name);

            }
            else
            {
                _players[i] = null;
            }
        }

        //updates camera
        _cameraController.UpdateCamera(_players);
    }

    private void RemovePlayer(Player player) //what happens when a player is dropped
    {
        Destroy(player.gameObject);
    }
        

    private void OnGUI()
    {
        //adding players debugging
        if(GUILayout.Button("Add Player"))
        {
            AddPlayer(SelectRandomClassFromAvailable());
        }
        //removing players debugging
        if(_players != null && _players.Length == 4)
        {
            if (_players[0])
            {
                if(GUILayout.Button("Drop Player 1"))
                {
                    DropPlayer(1);
                }
            }
            if (_players[1])
            {
                if (GUILayout.Button("Drop Player 2"))
                {
                    DropPlayer(2);
                }
            }
            if (_players[2])
            {
                if (GUILayout.Button("Drop Player 3"))
                {
                    DropPlayer(3);
                }
            }
            if (_players[3])
            {
                if (GUILayout.Button("Drop Player 4"))
                {
                    DropPlayer(4);
                }
            }
        }

    }
}
