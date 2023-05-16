using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

//manages controllers for each player using controller IDs
public class ControllerManager : MonoBehaviour
{
    private static readonly Dictionary<Gamepad, GamePadState> _gamePads = new Dictionary<Gamepad, GamePadState>();
    private static readonly List<Gamepad> _gamepadList = new List<Gamepad>();

    public static Action<Gamepad, GamePadButton> playerPadPressed;
    public static Action<Gamepad, Vector2> playerStickInput;

    public static Action<Gamepad, GamePadButton> selectionPadPressed;

    public static Action<Gamepad> unboundGamePadPressed;

    private static Vector2 _currentStickVector;
    private static GamePadState _currentState;
    private static GamePadButton _currentInput;

    //tracks inputs
    private void Update()
    {
        if(Gamepad.all.Count > _gamePads.Count)
        {
            foreach(Gamepad pad in Gamepad.all)
            {
                if(!_gamePads.ContainsKey(pad))
                {
                    _gamePads.Add(pad, new GamePadState(pad));
                    _gamepadList.Add(pad);
                }
            }
        }

        _gamePads.TryGetValue(Gamepad.current, out _currentState);

        if(_currentState != null)
        {
            if(_currentState.gamePadMode == GamePadMode.None)
            {
                unboundGamePadPressed?.Invoke(Gamepad.current);
            }
        }

        for (int i = 0; i < _gamepadList.Count; i++)
        {
            _currentState = TryGetGamepadState(_gamepadList[i]);

            if(_currentState != null)
            {
                _currentInput = CheckButtonInput(_gamepadList[i]);
                if (_currentInput != GamePadButton.None)
                {
                    switch (_currentState.gamePadMode)
                    {
                        case GamePadMode.SelectScreen:
                            selectionPadPressed?.Invoke(_gamepadList[i], _currentInput);
                        break;
                        case GamePadMode.InGame:
                            playerPadPressed?.Invoke(_gamepadList[i], _currentInput);
                            break;
                    }
                }
                _currentStickVector = CheckLeftStickInput(_gamepadList[i]);
                if(_currentState.gamePadMode == GamePadMode.InGame)
                {
                    playerStickInput?.Invoke(_gamepadList[i], CheckLeftStickInput(_gamepadList[i]));
                }
            }
        }
    }


    #region "Public Checking Functions"
    private static GamePadButton CheckButtonInput(Gamepad pad)
    {
        //face buttons
        if (pad.aButton.wasPressedThisFrame)
        {
            return GamePadButton.AButtonSouth;
        }
        if (pad.xButton.wasPressedThisFrame)
        {
            return GamePadButton.XButtonWest;
        }
        if (pad.bButton.wasPressedThisFrame)
        {
            return GamePadButton.BButtonEast;
        }
        //Dpad
        if (pad.dpad.down.wasPressedThisFrame)
        {
            return GamePadButton.DPadDown;
        }
        if (pad.dpad.left.wasPressedThisFrame)
        {
            return GamePadButton.DPadLeft;
        }
        if (pad.dpad.right.wasPressedThisFrame)
        {
            return GamePadButton.DPadRight;
        }
        //Start
        if (pad.startButton.wasPressedThisFrame)
        {
            return GamePadButton.Start;
        }

        return GamePadButton.None;
    }
    private static Vector2 CheckLeftStickInput(Gamepad pad)
    {
        if(pad.leftStick.right.isPressed)
        {
            _currentStickVector.x = 1;
        }
        else if(pad.leftStick.left.isPressed)
        {
            _currentStickVector.x = -1;
        }
        else
            _currentStickVector.x = 0;


        if (pad.leftStick.up.isPressed)
        {
            _currentStickVector.y = 1;
        }
        else if (pad.leftStick.down.isPressed)
        {
            _currentStickVector.y = -1;
        }
        else
            _currentStickVector.y = 0;

        return _currentStickVector;
    }

    #endregion

    #region "Public Functions"
    
    /// <summary>
    /// Gets the state information of a gamepad if it exsists
    /// </summary>
    /// <param name="gamepad"> gamepad to get information on </param>
    /// <returns> state information of the gamepad </returns>
    public static GamePadState TryGetGamepadState(Gamepad gamepad)
    {
        if(_gamePads.TryGetValue(gamepad, out GamePadState state))
        {
            return state;
        }
        return null;
    }

    #endregion

}

public class GamePadState
{
    public readonly Gamepad gamePad;

    public GamePadMode gamePadMode = GamePadMode.None;

    public PlayerInformationUI attatchedSelectionUI = null;
    public Player attatchedPlayer = null;

    public GamePadState(Gamepad pad)
    {
        gamePad = pad;
    }
}

public enum GamePadMode
{
    SelectScreen,
    InGame,
    None
}

public enum GamePadButton
{
    None,
    AButtonSouth, //south 
    XButtonWest, //west
    BButtonEast, //east
    DPadDown,
    DPadLeft,
    DPadRight,
    Start

}