using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    #region "Variables"
    [HideInInspector] public Player attatchedPlayer;

    private PlayerActionMap _playerActionMap;

    private int _controllerNumber;

    //variables used to store information internaly
    private Vector2 _movementVector = new Vector2();
    private Vector3 _appliedMovementVector = new Vector3();
    private Rigidbody _rBody;

    private GameObject _currentShot;
    private Rigidbody _currentShotRBody;
    private Projectile _currentprojectile;

    private bool _canShoot = true;

    //for shot input buffer
    private bool _shotInputBuffer = false;
    private Coroutine _shotBufferCoroutine = null;
    private int _recentDeviceID;
    #endregion

    #region "Unity Funtions"

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

            _playerActionMap.PlayerMovement.ControllerButtonsShoot.performed += context => PlayerShoot(context, false);

            _playerActionMap.PlayerMovement.ControllerButtonsPotion.performed += context => PlayerUsePotion(context);
            _playerActionMap.PlayerMovement.ControllerButtonsKey.performed += context => PlayerUseKey(context);
        }

    }

    private void OnDisable()
    {
        //player action unsubscriptions
        if (_playerActionMap != null)
        {
            _playerActionMap.PlayerMovement.ControllerButtonsShoot.performed -= context => PlayerShoot(context, false);
            _playerActionMap.PlayerMovement.ControllerButtonsPotion.performed -= context => PlayerUsePotion(context);
            _playerActionMap.PlayerMovement.ControllerButtonsKey.performed -= context => PlayerUseKey(context);

            _playerActionMap.Disable();
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }
    #endregion

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
    private void PlayerShoot(InputAction.CallbackContext context, bool buffered)
    {
        //sets the lastcontext variable for use in the input buffering
        if (!buffered)
        {
            _recentDeviceID = context.control.device.deviceId;
        }

        //can't shoot, starts buffer and returns. mannualy set with a literal to a fith of a second
        if (!_canShoot)
        {
            //input buffer coroutine
            if (_shotBufferCoroutine != null)
            {
                StopCoroutine(_shotBufferCoroutine);
            }

            _shotBufferCoroutine = StartCoroutine(ShotInputBuffer(0.20f));
            return;
        }

        //handles the creation of the shot projectile 
        if (buffered || ControllerManager.ButtonPressed(context, (PlayerNums)_controllerNumber))
        {

            if (attatchedPlayer.ClassData.CharacterShotPrefab != null)
            {
                // returns out of a pool already exsists of the type so its fine
                ObjectPooling.MakeNewObjectPool(attatchedPlayer.ClassData.CharacterShotPrefab, 10);

                _currentShot = ObjectPooling.PullObjectFromPool(attatchedPlayer.ClassData.CharacterShotPrefab);

                if (_currentShot != null)
                {
                    //activates and launches projectile
                    _currentShot.transform.position = transform.position;
                    _currentShot.transform.LookAt(transform.position + transform.forward);

                    //launches the projectile
                    _currentShot.TryGetComponent(out _currentShotRBody);
                    if (_currentShotRBody)
                    {
                        _currentShotRBody.velocity = transform.forward * attatchedPlayer.ClassData.BaseShotSpeed;
                    }

                    //tells the projectile that this player is the source of the projectile
                    _currentShot.TryGetComponent(out _currentprojectile);
                    if (_currentprojectile)
                    {
                        _currentprojectile.InitilizeProjectile(gameObject, (int)attatchedPlayer.PlayerStats.GetPlayerStat(PlayerStatCategories.ShotDamage), ProjectileSourceType.Player);
                    }
                }
            }
        }

        //starts the shot delay
        StartCoroutine(ShotDelayTimer(attatchedPlayer.PlayerStats.GetPlayerStat(PlayerStatCategories.ShotDowntime)));
    }
    /// <summary>
    /// Player potion action invoked by a controller
    /// </summary>
    private void PlayerUsePotion(InputAction.CallbackContext context)
    {
        if (ControllerManager.ButtonPressed(context, (PlayerNums)_controllerNumber))
        {
            if (attatchedPlayer.PlayerInventory != null)
            {
                PotionInventoryItem._potionDamage = (int)attatchedPlayer.PlayerStats.GetPlayerStat(PlayerStatCategories.MagicDamage);
                attatchedPlayer.PlayerInventory.TryUseItem(ItemType.Potion);
            }

        }
    }
    /// <summary>
    /// Player key action invoked by a controller
    /// </summary>
    private void PlayerUseKey(InputAction.CallbackContext context)
    {
        if (ControllerManager.ButtonPressed(context, (PlayerNums)_controllerNumber))
        {
            if (attatchedPlayer.PlayerInventory != null)
            {
                if (attatchedPlayer.ClosestDoor != null && Vector3.Distance(transform.position, attatchedPlayer.ClosestDoor.transform.position) < 4)
                {
                    if(!attatchedPlayer.ClosestDoor.IsOpened)
                    {
                        if(attatchedPlayer.PlayerInventory.TryUseItem(ItemType.Key))
                            attatchedPlayer.ClosestDoor.OpenDoor();
                    }
                }
            }
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
        if (_playerActionMap == null)
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

    #region"Coroutines"
    //Shooting input delay and buffering coroutines
    private IEnumerator ShotDelayTimer(float timerDuration)
    {
        _canShoot = false;

        yield return new WaitForSeconds(timerDuration);

        _canShoot = true;

        if (_shotInputBuffer)
        {
            if (ControllerManager.ButtonPressed(_recentDeviceID, (PlayerNums)_controllerNumber))
            {
                _shotInputBuffer = false;
                PlayerShoot(new InputAction.CallbackContext(), true);
            }
        }
    }

    private IEnumerator ShotInputBuffer(float duration)
    {
        _shotInputBuffer = true;
        yield return new WaitForSeconds(duration);
        _shotInputBuffer = false;
    }
    #endregion
}