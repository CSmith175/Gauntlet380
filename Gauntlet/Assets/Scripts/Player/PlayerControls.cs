using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [HideInInspector] public Player attatchedPlayer;

    private PlayerActionMap _playerActionMap;

    private int _controllerNumber;

    //variables used to store information internaly
    private Vector2 _movementVector = new Vector2();
    private Vector3 _appliedMovementVector = new Vector3();
    private Rigidbody _rBody;

    private void OnEnable()
    {
        //initilizes rigidbody
        if (!gameObject.TryGetComponent(out _rBody))
        {
            _rBody = gameObject.AddComponent<Rigidbody>();
        }
        InitilizeRigidbody(_rBody);




        //player action subscriptions
        if (_playerActionMap != null)
        {
            _playerActionMap.Enable();

            _playerActionMap.PlayerMovement.ControllerButtonsShoot.performed += context => PlayerShoot(context);
            _playerActionMap.PlayerMovement.ControllerButtonsPotion.performed += context => PlayerUsePotion(context);
            _playerActionMap.PlayerMovement.ControllerButtonsKey.performed += context => PlayerUseKey(context);
        }

    }

    private void OnDisable()
    {
        //player action unsubscriptions
        if(_playerActionMap != null)
        {
            _playerActionMap.PlayerMovement.ControllerButtonsShoot.performed -= context => PlayerShoot(context);
            _playerActionMap.PlayerMovement.ControllerButtonsPotion.performed -= context => PlayerUsePotion(context);
            _playerActionMap.PlayerMovement.ControllerButtonsKey.performed -= context => PlayerUseKey(context);

            _playerActionMap.Disable();
        }
    }



    private void FixedUpdate()
    {
        PlayerMove();
    }

    #region Player Actions

    /// <summary>
    /// Moves the player based on inputs
    /// </summary>
    private void PlayerMove()
    {
        //checks controller input
        _movementVector = ControllerManager.GetMovementVector((PlayerNums)_controllerNumber);

        //if its still 0, velocity is 0
        if (_movementVector.x == 0 && _movementVector.y == 0)
        {
            _rBody.velocity = Vector3.zero;
            return; //no movement
        }

        //player movement
        _appliedMovementVector.x = _movementVector.x;
        _appliedMovementVector.y = 0;
        _appliedMovementVector.z = _movementVector.y;

        _rBody.velocity = Time.deltaTime * attatchedPlayer.PlayerStats.GetPlayerStat(PlayerStatCategories.MoveSpeed) * _appliedMovementVector;
        transform.LookAt(transform.position + _rBody.velocity);
    }

    //Buttons
    /// <summary>
    /// Player fire action invoked from a controller
    /// </summary>
    private void PlayerShoot(InputAction.CallbackContext context)
    {
        if (ControllerManager.ButtonPressed(context, (PlayerNums)_controllerNumber))
        {
            //Debug.Log("Player shoot from controller");
            
        }
    }
    /// <summary>
    /// Player potion action invoked by a controller
    /// </summary>
    private void PlayerUsePotion(InputAction.CallbackContext context)
    {
        if (ControllerManager.ButtonPressed(context, (PlayerNums)_controllerNumber))
        {
            Debug.Log("Player Used Potion from controller");
        }
    }
    /// <summary>
    /// Player key action invoked by a controller
    /// </summary>
    private void PlayerUseKey(InputAction.CallbackContext context)
    {
        if (ControllerManager.ButtonPressed(context, (PlayerNums)_controllerNumber))
        {
            Debug.Log("Player Used Key from controller");
        }
    }

    #endregion

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

        //determines controller
        ControllerManager.BindRandomAvailableControllerToPlayer((PlayerNums)playerNumber);
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

        //constraints
        rBody.constraints = RigidbodyConstraints.FreezePositionY;
        rBody.freezeRotation = true;
    }

    #endregion

}
