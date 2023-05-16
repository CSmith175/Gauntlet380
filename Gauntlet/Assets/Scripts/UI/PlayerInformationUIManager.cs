using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

//manages the 4 PlayerInormationUIs, communicates with the player manager
public class PlayerInformationUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerInformationUIPrefab;
    private PlayerInformationUI[] _playerUIInformations;

    //Internaly used variables
    private GameObject _currentPlayerInformationObj;
    private PlayerInformationUI _currentUIInformation;

    //for join input
    private PlayerActionMap _actionmap;

    #region "Unity Functions"
    //sets up player UI
    private void Awake()
    {
        InitlizePlayerInformationUIManger(4);
    }

    //subscribes to event bus event for when players join and leave, annd join inputs
    private void OnEnable()
    {
        EventBus.OnPlayerChanged.AddListener(FigureOutPlayers);

        if (_actionmap == null)
        {
            _actionmap = new PlayerActionMap();
        }

        _actionmap.Enable();
    }
    private void OnDisable()
    {
        EventBus.OnPlayerChanged.RemoveListener(FigureOutPlayers);

        if (_actionmap != null)
        {
            _actionmap.Disable();
        }
    }

    private void Start()
    {
        if (_actionmap != null)
        {
            _actionmap.PlayerMovement.ControllerButtonsClassScroll.performed += context => ClassScroll(context);
            _actionmap.PlayerMovement.ControllerButtonsClassSelect.performed += context => ClassSelect(context);
        }
    }

    #endregion

    #region "Event Subscription Functions"
    private void FigureOutPlayers(Player[] players)
    {
        if (players != null && _playerUIInformations != null)
        {

            for (int i = 0; i < players.Length && i < _playerUIInformations.Length; i++)
            {
                if(players[i] != null)
                {
                    _playerUIInformations[i].UpdateUIState(PlayerUIDisplayState.Game, players[i]);
                    _playerUIInformations[i].SetUIFromCharacter(players[i]);
                }
                else
                {
                    _playerUIInformations[i].UpdateUIState(PlayerUIDisplayState.Unjoined, null);
                }

            }
        }
    }

    #endregion

    #region "Helper Functions
    //Initilizes the Player Information UI from the prefab
    private void InitlizePlayerInformationUIManger(int playerCap)
    {
        if (_playerInformationUIPrefab)
        {
            _playerUIInformations = new PlayerInformationUI[playerCap];
            for (int i = 1; i < playerCap + 1; i++)
            {
                _currentPlayerInformationObj = Instantiate(_playerInformationUIPrefab, transform);
                _currentPlayerInformationObj.name = "Player UI Information: " + i.ToString();

                _currentPlayerInformationObj.TryGetComponent(out _currentUIInformation);

                if (_currentUIInformation != null)
                {
                    _currentUIInformation.UpdateUIState(PlayerUIDisplayState.Unjoined, null);
                    _playerUIInformations[i - 1] = _currentUIInformation;
                }

                else
                    Debug.LogWarning("UI Information prefab for the players on the PlayerInformationUIManager (Gameobject: " + gameObject.name + ") didn't have a PlayerInformationUI.cs attatched, please attatch one)");
            }

            _currentUIInformation = null;
            _currentPlayerInformationObj = null;
        }
    }

    #endregion

    #region "Class Select Functions
    private void ClassScroll(InputAction.CallbackContext context)
    {

        //gets the float value
        int value = (int)context.ReadValue<float>();
        //input
        if (_playerUIInformations != null)
        {
            if (!ControllerManager.CheckControllerActive(context.control.device.deviceId))
            {
                //binding found
                if(ControllerManager.CheckControllerPlayerSelect(context.control.device.deviceId))
                {
                    _currentUIInformation = ControllerManager.TryGetPlayerSelectBinding(context.control.device.deviceId);
                }
                //no binding found, try to find first unjoined
                else
                {
                    for (int i = 0; i < _playerUIInformations.Length; i++)
                    {
                        if(_playerUIInformations[i].State == PlayerUIDisplayState.Unjoined && !ControllerManager.CheckIfUIBound(_playerUIInformations[i]))
                        {
                            ControllerManager.BindInactivePlayerJoinController(context.control.device.deviceId, _playerUIInformations[i]);
                            _currentUIInformation = ControllerManager.TryGetPlayerSelectBinding(context.control.device.deviceId);
                        }
                    }
                }
            }
        }

        if(_currentUIInformation != null)
        {
            _currentUIInformation.UpdateCurrentSelectedClass(value);
        }
        

    }
    private void ClassSelect(InputAction.CallbackContext context)
    {
        if (_playerUIInformations != null)
        {
            if (!ControllerManager.CheckControllerActive(context.control.device.deviceId))
            {
                //binding found
                if (ControllerManager.CheckControllerPlayerSelect(context.control.device.deviceId))
                {
                    Debug.Log("binding found!");
                    _currentUIInformation = ControllerManager.TryGetPlayerSelectBinding(context.control.device.deviceId);
                }
                //no binding found, try to find first unjoined
                else
                {
                    Debug.Log("No binding found");
                    for (int i = 0; i < _playerUIInformations.Length; i++)
                    {
                        if (_playerUIInformations[i].State == PlayerUIDisplayState.Unjoined && !ControllerManager.CheckIfUIBound(_playerUIInformations[i]))
                        {
                            ControllerManager.BindInactivePlayerJoinController(context.control.device.deviceId, _playerUIInformations[i]);
                            _currentUIInformation = ControllerManager.TryGetPlayerSelectBinding(context.control.device.deviceId);
                        }
                    }
                }
            }
        }

        if(_currentUIInformation != null)
        {
            ClassData data = _currentUIInformation.GetSelectedClass();

            if(data)
            {
                EventBus.OnTryAddPlayer?.Invoke(context.control.device.deviceId, data);
            }
        }
    }
    #endregion


}
