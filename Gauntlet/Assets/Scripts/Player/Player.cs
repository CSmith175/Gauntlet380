using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerActionMap _playerActionMap;
    private InputAction _playerInputAction;

    private int _controllerNumber;
    public int ControllerNumber
    {
        get { return _controllerNumber; }
    }

    private ClassData _classData;
    public ClassData ClassData
    {
        get { return _classData; }
    }

    private enum PlayerNums
    {
        Player1 = 1,
        Player2 = 2,
        Player3 = 3,
        Player4 = 4
    }


    public void InitilizePlayer(int controllerNumber, ClassData classData)
    {
        _controllerNumber = Mathf.Clamp(controllerNumber, 1, 4);
        DeterminePlayerAction(_controllerNumber);

        InitilizeInputAction();
        _classData = classData;
        OnEnable();
    }

    private void OnEnable()
    {
        if(_playerActionMap != null)
            _playerActionMap.Enable();

    }

    private void OnDisable()
    {
        if (_playerActionMap != null)
            _playerActionMap.Disable();
    }

    private void Update()
    {
        if (_playerInputAction != null) //temp move check
        {
            if (_playerInputAction.ReadValue<Vector2>() != Vector2.zero)
            {

                Vector2 inputVector = _playerInputAction.ReadValue<Vector2>();
                Vector3 changeInPosition = Vector3.zero;

                changeInPosition.x = inputVector.x;
                changeInPosition.z = inputVector.y;

                if (_classData)
                    transform.position += (changeInPosition * _classData.BaseMoveSpeed * Time.deltaTime);
                else
                    Debug.LogError("No class data could be found on Player " + _controllerNumber);
            }
        }
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
