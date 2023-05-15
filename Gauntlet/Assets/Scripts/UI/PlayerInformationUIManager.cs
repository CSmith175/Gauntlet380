using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//manages the 4 PlayerInormationUIs, communicates with the player manager
public class PlayerInformationUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerInformationUIPrefab;
     private PlayerInformationUI[] _playerUIInformations;

    //Internaly used variables
    private GameObject _currentPlayerInformationObj;
    private PlayerInformationUI _currentUIInformation;

    //sets up player UI
    private void Awake()
    {
        InitlizePlayerInformationUIManger(4);
    }

    //subscribes to event bus event for when players join and leave
    private void OnEnable()
    {
        EventBus.OnPlayerChanged.AddListener(FigureOutPlayers);
    }
    private void OnDisable()
    {
        EventBus.OnPlayerChanged.RemoveListener(FigureOutPlayers);
    }


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

    //sorts players lowest to highest (first to last) by their controller num. controller num is used to keep placement consistent.
    private int SortPlayersByControllerNum(Player a, Player b)
    {
        if (a == null && b != null)
            return 1;
        if (a != null && b == null)
            return -1;

        if (a == null && b == null)
            return 0;

        if (a.ControllerNumber > b.ControllerNumber)
            return 1;
        if (a.ControllerNumber < b.ControllerNumber)
            return -1;

        return 0;
    }
    #endregion







}
