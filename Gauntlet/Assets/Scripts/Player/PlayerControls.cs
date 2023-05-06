using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [HideInInspector] public Player attatchedPlayer;

    private PlayerActionMap _playerActionMap;
    private InputAction _playerInputAction;

    private int _controllerNumber;

    //variables used to store information internaly
    private Vector2 _movementVector = new Vector3();
    private Vector3 _appliedMovementVector = new Vector3();
    private Vector3 _turnVelocity = new Vector3();
    private Rigidbody _rBody;

    private void OnEnable()
    {
        if (_playerActionMap != null)
            _playerActionMap.Enable();
        //_playerInputAction.performed += contet => PlayerMove(contet);
        //_playerInputAction.started += contet => PlayerMove(contet);
        //_playerInputAction.canceled += contet => PlayerMove(contet);


        //initilizes rigidbody
        if (!gameObject.TryGetComponent(out _rBody))
        {
            _rBody = gameObject.AddComponent<Rigidbody>();
        }
        InitilizeRigidbody(_rBody);
    }

    private void OnDisable()
    {
        if (_playerActionMap != null)
            _playerActionMap.Disable();

        //_playerInputAction.performed -= contet => PlayerMove(contet);
        //_playerInputAction.started -= contet => PlayerMove(contet);
        //_playerInputAction.canceled -= contet => PlayerMove(contet);
    }



    private void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if(_playerActionMap != null && _playerInputAction != null)
        {
            _movementVector = _playerInputAction.ReadValue<Vector2>();
            {
                if (_movementVector.x == 0 && _movementVector.y == 0)
                {
                    _rBody.velocity = Vector3.zero;
                    return; //no movement
                }


                //player movement
                _appliedMovementVector.x = _movementVector.x;
                _appliedMovementVector.y = 0;
                _appliedMovementVector.z = _movementVector.y;

                _rBody.velocity = Time.deltaTime * 150 * attatchedPlayer.PlayerStats.GetPlayerStat(PlayerStatCategories.MoveSpeed) * _appliedMovementVector;

                //player rotation
                //transform.LookAt(Vector3.SmoothDamp(transform.position + transform.forward, transform.position + _rBody.velocity, ref _turnVelocity, 0.05f));
                transform.LookAt(transform.position + _rBody.velocity);
            }
        }
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


    #region "Initilization Related"

    public void InitilizePlayer(int controllerNumber, Player player)
    {
        InitilizeInputAction();

        _controllerNumber = Mathf.Clamp(controllerNumber, 1, 4);
        DeterminePlayerAction(_controllerNumber);
        attatchedPlayer = player;

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

    private void InitilizeRigidbody(Rigidbody rBody)
    {
        rBody.useGravity = false;
        rBody.interpolation = RigidbodyInterpolation.Interpolate;
        rBody.freezeRotation = true;
    }

    #endregion


}
