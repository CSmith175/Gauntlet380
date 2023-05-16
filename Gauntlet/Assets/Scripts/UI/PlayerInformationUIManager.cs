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

    //gamepads
    private Dictionary<Gamepad, PlayerInformationUI> _controlBindings = new Dictionary<Gamepad, PlayerInformationUI>();

    #region "Unity Functions"
    //sets up player UI
    private void Awake()
    {
        InitlizePlayerInformationUIManger(4);
    }

    //subscribes to event bus event for when players join and leave, annd join inputs
    private void OnEnable()
    {
        //controller stuff
        ControllerManager.unboundGamePadPressed += BindUnboundControllerToSelection;


        ControllerManager.selectionPadPressed += ClassScroll;
        ControllerManager.selectionPadPressed += ClassSelect;

    }
    private void OnDisable()
    {
        //controller stuff
        ControllerManager.unboundGamePadPressed -= BindUnboundControllerToSelection;

        ControllerManager.selectionPadPressed -= ClassScroll;
        ControllerManager.selectionPadPressed -= ClassSelect;
    }


    #endregion

    #region "Event Subscription Functions"
    private void BindUnboundControllerToSelection(Gamepad gamepad)
    {
        GamePadState gamePadstate = ControllerManager.TryGetGamepadState(gamepad);
        if(gamePadstate != null && _playerUIInformations != null)
        {
            for (int i = 0; i < _playerUIInformations.Length; i++)
            {
                //gets first Unjoined UI
                if (_playerUIInformations[i].State == PlayerUIDisplayState.Unjoined && _playerUIInformations[i].isSelectionBoundToController == false)
                {
                    gamePadstate.attatchedSelectionUI = _playerUIInformations[i];
                    gamePadstate.gamePadMode = GamePadMode.SelectScreen;

                    _playerUIInformations[i].isSelectionBoundToController = true;
                    _playerUIInformations[i].boundGamePad = gamepad;

                    if(_controlBindings.ContainsKey(gamepad))
                    {
                        _controlBindings.Remove(gamepad);
                    }
                    _controlBindings.Add(gamepad, _playerUIInformations[i]);


                    Debug.Log("Bound");
                    return;
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
    private void ClassScroll(Gamepad pad, GamePadButton button)
    {
        if(_controlBindings.ContainsKey(pad))
        {
            _controlBindings.TryGetValue(pad, out _currentUIInformation);

            if (_currentUIInformation != null)
            {
                int value;

                if (button == GamePadButton.DPadLeft)
                    value = -1;
                else if (button == GamePadButton.DPadRight)
                    value = 1;
                else
                    return;

                _currentUIInformation.UpdateCurrentSelectedClass(value);
            }
        }

    }
    private void ClassSelect(Gamepad pad, GamePadButton button)
    {
        if (_controlBindings.ContainsKey(pad))
        {
            _controlBindings.TryGetValue(pad, out _currentUIInformation);

            if (_currentUIInformation != null)
            {
                if (button == GamePadButton.Start || button == GamePadButton.DPadDown || button == GamePadButton.AButtonSouth)
                {
                    ClassData data = _currentUIInformation.GetSelectedClass();

                    if (data && _currentUIInformation.boundGamePad != null)
                    {
                        EventBus.OnTryAddPlayer?.Invoke(data, _currentUIInformation.boundGamePad);

                        _currentUIInformation.isSelectionBoundToController = false;
                        _currentUIInformation.boundGamePad = null;

                        if (PlayerManager.playerAwaitingUI != null)
                            _currentUIInformation.UpdateUIState(PlayerUIDisplayState.Game, PlayerManager.playerAwaitingUI);
                        PlayerManager.playerAwaitingUI = null;

                    }
                }
            }
        }
    }
    #endregion


}
