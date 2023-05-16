using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//manages controllers for each player using controller IDs
public class ControllerManager : MonoBehaviour
{
    //staticly tracked dictionary and list for controller bindings
    private static readonly Dictionary<PlayerNums, Gamepad> _playerControllers = new Dictionary<PlayerNums, Gamepad>();
    private static readonly List<Gamepad> _inUseGamepads = new List<Gamepad>();
    private static readonly Dictionary<int, Gamepad> _gamePadDeviceIDs = new Dictionary<int, Gamepad>();

    //staticaly tracked information for player selection controller bindings
    private static readonly Dictionary<int, PlayerInformationUI> _playerSelectBindings = new Dictionary<int, PlayerInformationUI>();
    private static readonly List<PlayerInformationUI> _boundUI = new List<PlayerInformationUI>();

    //private variables
    private static Gamepad _currentController;
    private static Gamepad _checkingController;
    private static Vector2 _currentMovementVector = new Vector2();
    private static PlayerInformationUI _currentInformationUI;

    #region "Controller Binding Functions

    /// <summary>
    /// Binds a random controller to the given player
    /// </summary>
    /// <param name="playerNum"> Player number </param>
    public static void BindRandomAvailableControllerToPlayer(PlayerNums playerNum)
    {
        //removes potential current controller binding
        UnbindPlayerController(playerNum);


        //gets the first available controller on Gamepad.all
        foreach (Gamepad gamePad in Gamepad.all)
        {
            if (!_inUseGamepads.Contains(gamePad))
            {
                _playerControllers.Add(playerNum, gamePad);
                _inUseGamepads.Add(gamePad);
                _gamePadDeviceIDs.Add(gamePad.device.deviceId, gamePad);
            }
        }

    }

    /// <summary>
    ///  Binds a specific controller to a player, if controller inputted is invalid or already taken assigns a random available controller
    /// </summary>
    /// <param name="playerNum"> Player number </param>
    /// <param name="gamePad"> Controller to bind </param>
    public static void BindSpecificAvailableControllerToPlayer(PlayerNums playerNum, Gamepad gamePad)
    {
        //removes potential current controller binding
        UnbindPlayerController(playerNum);
        //unbinds potential player selection binding
        UnbindInactivePlayerJoinController(gamePad.deviceId);

        //adds specific gamepad to bindings
        if(!_inUseGamepads.Contains(gamePad))
        {
            _playerControllers.Add(playerNum, gamePad);
            _inUseGamepads.Add(gamePad);
            _gamePadDeviceIDs.Add(gamePad.device.deviceId, gamePad);
        }
        else
        {
            //if the gamepad assigned was in use defaults back to assigning a random controller

            Debug.LogWarning("Game pad given to player was already in use, assigning random gamepad instead");
            BindRandomAvailableControllerToPlayer(playerNum);
        }
    }

    /// <summary>
    /// Unbinds a controller from a player
    /// </summary>
    /// <param name="playerNum"> Player number </param>
    public static void UnbindPlayerController(PlayerNums playerNum)
    {
        //removes potential current controller binding
        if (_playerControllers.ContainsKey(playerNum))
        {
            _playerControllers.TryGetValue(playerNum, out _currentController);

            _inUseGamepads.Remove(_currentController);
            _gamePadDeviceIDs.Remove(_currentController.device.deviceId);

            _playerControllers.Remove(playerNum);
        }
    }

    //for "inactive" controllers (Player Joining)
    /// <summary>
    /// Binds a controller for use in player selection
    /// </summary>
    /// <param name="deviceID"> ID of the controller </param>
    /// <param name="joinInputs"> attatcked JoinInputs instance </param>
    public static void BindInactivePlayerJoinController(int deviceID, PlayerInformationUI playerUI)
    {
        if(playerUI != null && !_playerSelectBindings.ContainsKey(deviceID))
        {
            _playerSelectBindings.Add(deviceID, playerUI);
            _boundUI.Add(playerUI);
        }

    }

    /// <summary>
    /// Removes the inactive binding for use in player selection
    /// </summary>
    /// <param name="deviceID"> ID of the controller </param>
    public static void UnbindInactivePlayerJoinController(int deviceID)
    {
        if (_playerSelectBindings.ContainsKey(deviceID))
        {
            _playerSelectBindings.TryGetValue(deviceID, out _currentInformationUI);
            _boundUI.Remove(_currentInformationUI);

            _playerSelectBindings.Remove(deviceID);
        }

    }

    #endregion

    #region "Input Retreiving Functions"

    /// <summary>
    /// Gets the movement vector of a player from their controller, (0,0) to (1,1)
    /// </summary>
    /// <param name="playerNum"> Player number </param>
    /// <returns></returns>
    public static Vector2 GetMovementVector(PlayerNums playerNum)
    {
        if(_playerControllers.TryGetValue(playerNum, out _currentController))
        {
            //y(z) axis
            if (_currentController.leftStick.up.isPressed)
                _currentMovementVector.y = 1;
            else if (_currentController.leftStick.down.isPressed)
                _currentMovementVector.y = -1;
            else
                _currentMovementVector.y = 0;

            //x axis
            if (_currentController.leftStick.right.isPressed)
                _currentMovementVector.x = 1;
            else if (_currentController.leftStick.left.isPressed)
                _currentMovementVector.x = -1;
            else
                _currentMovementVector.x = 0;

            return _currentMovementVector;
        }

        return Vector2.zero;
    }

    /// <summary>
    /// Figures out which gamepad had a button pressed from a CallbackContext and a playerNum
    /// </summary>
    public static bool ButtonPressed(InputAction.CallbackContext context, PlayerNums playerNum)
    {
        if (_gamePadDeviceIDs.TryGetValue(context.control.device.deviceId, out _checkingController))
        {
            if(_playerControllers.TryGetValue(playerNum, out _currentController))
            {
                if(_currentController == _checkingController)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Figures out which gamepad had a button pressed from a deviceID and a playerNum
    /// </summary>
    public static bool ButtonPressed(int deviceID, PlayerNums playerNum)
    {
        _gamePadDeviceIDs.TryGetValue(deviceID, out _currentController);
        _playerControllers.TryGetValue(playerNum, out _checkingController);

        if (_currentController != null)
        {
            if (_currentController == _checkingController)
            {
                return true;
            }
        }
        return false;
    }

    //for "inactive" controllers (Player Joining)

    /// <summary>
    /// Checks if a controller is in use by one of the players currently in-game
    /// </summary>
    /// <param name="deviceID"> device ID of the controller being tested </param>
    /// <returns> true if controller is in use, false if it's not </returns>
    public static bool CheckControllerActive(int deviceID)
    {
        if(_gamePadDeviceIDs.TryGetValue(deviceID, out _currentController))
        {
            return true;
        }

        return false;
    }

    public static bool CheckControllerPlayerSelect(int deviceID)
    {
        if (_playerSelectBindings.TryGetValue(deviceID, out _currentInformationUI))
        {
            return true;
        }

        return false;
    }

    public static PlayerInformationUI TryGetPlayerSelectBinding(int deviceID)
    {
        if(_playerSelectBindings.TryGetValue(deviceID, out _currentInformationUI))
        {
            return _currentInformationUI;
        }

        return null;
    }

    public static bool CheckIfUIBound(PlayerInformationUI playerUI)
    {
        if (_boundUI.Contains(playerUI))
            return true;

        return false;
    }

    #endregion
}
