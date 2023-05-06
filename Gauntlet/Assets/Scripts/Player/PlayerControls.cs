using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private PlayerActionMap _playerActionMap;
    private InputAction _playerInputAction;

    private int _controllerNumber;

    private void OnEnable()
    {
        if (_playerActionMap != null)
            _playerActionMap.Enable();
    }

    private void OnDisable()
    {
        if (_playerActionMap != null)
            _playerActionMap.Disable();
    }



    private void Update()
    {
        TempMovementFunction();
    }

    private void TempMovementFunction()
    {
        if (_playerActionMap != null && _playerInputAction != null)
        {
            Vector2 moveVector = _playerInputAction.ReadValue<Vector2>();
            Vector3 worldMoveVector = new Vector3();
            worldMoveVector.x = moveVector.x;
            worldMoveVector.z = moveVector.y;

            transform.position += (worldMoveVector * Time.deltaTime * 15);
        }
    }



    public void InitilizePlayer(int controllerNumber, ClassData classData)
    {
        InitilizeInputAction();

        _controllerNumber = Mathf.Clamp(controllerNumber, 1, 4);
        DeterminePlayerAction(_controllerNumber);

        OnEnable();
    }

    private void DeterminePlayerAction(int playerNumber)
    {
        if(_playerActionMap == null)
        {
            InitilizeInputAction();
        }

        playerNumber = Mathf.Clamp(playerNumber, 1, 4);

        switch ((PlayerNums)playerNumber)
        {
            case PlayerNums.Player1:
                _playerInputAction = _playerActionMap.PlayerMovement.Player1;
                break;
            case PlayerNums.Player2:
                _playerInputAction = _playerActionMap.PlayerMovement.Player2;
                break;
            case PlayerNums.Player3:
                _playerInputAction = _playerActionMap.PlayerMovement.Player3;
                break;
            case PlayerNums.Player4:
                _playerInputAction = _playerActionMap.PlayerMovement.Player4;
                break;
            default:
                break;
        }
    }

    private void InitilizeInputAction() //initilizes the action map
    {
        _playerActionMap = new PlayerActionMap();
        _playerActionMap.Enable();
    }
}
