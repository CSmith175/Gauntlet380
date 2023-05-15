using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformationUI : MonoBehaviour
{
    //current state of the UI
    private PlayerUIDisplayState _state;
    //player corresponds too. 
    private Player _player;

    private List<Image> _lightUIImages;
    private List<Image> _darkUIImages;

    #region "UI Components"

    #region "Unjoined UI"
    [Header("Unjoined UI")]
    [Space(10)]
    [Tooltip("Parent Object of the Unjoined UI")] [SerializeField] private GameObject _unjoinedUI;
    [Space(5)]

    #endregion

    #region "Game UI"
    [Header("Game UI")]
    [Space(10)]
    [Tooltip("Parent Object of the game UI")] [SerializeField] private GameObject _gameUI;
    [Space(5)]

    [Tooltip("Text for the player's score")] [SerializeField] private Text _scoreText;
    [Tooltip("Text for the player's health")] [SerializeField] private Text _healthText;


    [Tooltip("Background of the game UI (Tinted Dark)")] [SerializeField] private Image _gameUIBackground;

    [Tooltip("Text that displays the player number and class name")] [SerializeField] private Text _playerInfoText;


    [Tooltip("Icon for an empty inventory slot")] [SerializeField] private Sprite _emptyInventoryIcon;
    [Tooltip("Icon for a potion")] [SerializeField] private Sprite _potionIcon;
    [Tooltip("Icon for a key")] [SerializeField] private Sprite _keyIcon;

    [Tooltip("Transform of the UI component for displaying the player's UI")] [SerializeField] private Transform _inventoryDisplayGroup;
    private Image[] _inventoryImages; //images for the inventory
    private ItemType[] _currentInventory;
    private Image _currentImage;

    #endregion

    #region "Dead UI"
    [Header("Dead UI")]
    [Space(10)]
    [Tooltip("Parent Object of the DeadUI")] [SerializeField] private GameObject _deadUI;
    [Space(5)]

    #endregion

    #region "Level Complete"
    [Header("Level Complete UI")]
    [Space(10)]
    [Tooltip("Parent Object of the Level Complete UI")] [SerializeField] private GameObject _levelCompleteUI;


    #endregion

    #endregion


    private void Awake()
    {
        //adds images to dark color list
        if (_darkUIImages == null)
            _darkUIImages = new List<Image>();
        //image additions
        {
            //overall game background
            if (_gameUIBackground != null)
                _darkUIImages.Add(_gameUIBackground);
        }

        if (_lightUIImages == null)
            _lightUIImages = new List<Image>();
        //image additions
        {
            //inventory background
            if (_inventoryDisplayGroup != null)
                if (_inventoryDisplayGroup.TryGetComponent(out Image image))
                    _lightUIImages.Add(image);
        }

    }

    /// <summary>
    /// Initilizes canvas information to a player character, updates things like name and colors
    /// </summary>
    /// <param name="player"> inputted player, does nothing if null </param>
    public void SetUIFromCharacter(Player player)
    {
        _player = player;

        if(_player)
        {
            if (_playerInfoText)
                _playerInfoText.text = _player.ClassData.CharacterName;
        }

        SetUIColors();
    }

    /// <summary>
    /// Sets UI state for given player, if player is null sets to unjoined
    /// </summary>
    public void UpdateUIState(PlayerUIDisplayState state, Player player)
    {
        if(_player == null)
        {
            if(player == null)
            {
                //no player in the UI either inputted or currently, sets to unjoined
                SwapCanvas(PlayerUIDisplayState.Unjoined);
                return;
            }
            else
            {
                _player = player;
                SetUIFromCharacter(_player); //sets up UI display from character info
                UpdateInventoryDisplay(_player.PlayerInventory);
            }
        }
        if(player == null)
        {
            SwapCanvas(PlayerUIDisplayState.Unjoined);
            return;
        }

        SwapCanvas(state);
    }

    #region "Inventory"

    private void UpdateInventoryDisplay(PlayerInventory inventory)
    {
        if(inventory != null)
        {
            _currentInventory = inventory.GetInventoryInformation();

            if (_lightUIImages == null)
                _lightUIImages = new List<Image>();

            //creates Icons
            if (_inventoryImages == null)
            {
                _inventoryImages = new Image[_currentInventory.Length];

                for (int i = 0; i < _inventoryImages.Length; i++)
                {
                    _inventoryImages[i] = CreateInventoryImageIcon();
                }
            }

            //additional icons needed (inventory expanded)
            if(_inventoryImages.Length < _currentInventory.Length)
            {
                Image[] inventoryHolder = _inventoryImages;
                _inventoryImages = new Image[_currentInventory.Length];

                for (int i = 0; i < _inventoryImages.Length; i++)
                {
                    if(i < inventoryHolder.Length)
                    {
                        _inventoryImages[i] = inventoryHolder[i];
                    }
                    else
                    {
                        _inventoryImages[i] = CreateInventoryImageIcon();
                    }
                }

                inventoryHolder = null;
            }
            //extra incons in array (inventory shrunk)
            if(_inventoryImages.Length > _currentInventory.Length)
            {
                Image[] inventoryHolder = _inventoryImages;
                _inventoryImages = new Image[_currentInventory.Length];

                for (int i = 0; i < _inventoryImages.Length; i++)
                {
                    _inventoryImages[i] = inventoryHolder[i];
                }

                inventoryHolder = null;
            }

            //sets inventory Icons
            for (int i = 0; i < _currentInventory.Length && i < _inventoryImages.Length; i++)
            {
                if(_inventoryImages[i] != null)
                {
                    switch (_currentInventory[i])
                    {
                        case ItemType.Empty:
                            if (_emptyInventoryIcon != null)
                                _inventoryImages[i].sprite = _emptyInventoryIcon;
                            break;
                        case ItemType.Potion:
                            if (_potionIcon != null)
                                _inventoryImages[i].sprite = _potionIcon;
                            break;
                        case ItemType.Key:
                            if (_keyIcon != null)
                                _inventoryImages[i].sprite = _keyIcon;
                            break;
                        default:
                            break;
                    }
                }
            }

            SetUIColors();
        }
    }

    #endregion


    #region "Helper Functions"

    //handles the switching of the UI componets for the state
    private void SwapCanvas(PlayerUIDisplayState state)
    {
        CanvasToggle(_state, false);
        CanvasToggle(state, true);
        _state = state;
    }

    //toggles between the states, setting the inputted one to the desired state
    private void CanvasToggle(PlayerUIDisplayState canvas, bool toggleState)
    {
        switch (canvas)
        {
            case PlayerUIDisplayState.Unjoined:
                if(_unjoinedUI)    
                    _unjoinedUI.SetActive(toggleState);
                break;
            case PlayerUIDisplayState.Game:
                if(_gameUI) 
                    _gameUI.SetActive(toggleState);
                break;
            case PlayerUIDisplayState.Dead:
                if(_deadUI)   
                    _deadUI.SetActive(toggleState);
                break;
            case PlayerUIDisplayState.LevelComplete:
                if(_levelCompleteUI)
                    _levelCompleteUI.SetActive(toggleState);
                break;
            default:
                break;
        }
    }

    private Image CreateInventoryImageIcon()
    {
        if(_inventoryDisplayGroup)
        {
            _currentImage = new GameObject().AddComponent<Image>();
            _currentImage.gameObject.name = "Inventory Icon";

            _currentImage.rectTransform.sizeDelta = new Vector2(25, 25);
            _currentImage.rectTransform.anchorMin = Vector2.up;
            _currentImage.rectTransform.anchorMax = Vector2.up;
            _currentImage.rectTransform.pivot = Vector2.one / 2;

            _currentImage.transform.SetParent(_inventoryDisplayGroup);

            _lightUIImages.Add(_currentImage);

            return _currentImage;
        }

        Debug.LogWarning("No Inventory holder assigned on PlayerInformationUI.cs on: " + gameObject.name);
        return null;
    }

    private void SetUIColors()
    {
        if(_player)
        {
            //tints light images
            if (_lightUIImages != null)
            {
                for (int i = 0; i < _lightUIImages.Count; i++)
                {
                    if (_lightUIImages[i] != null)
                        _lightUIImages[i].color = _player.ClassData.CharacterUIColorLight;
                }
            }
            //tints dark images
            if (_darkUIImages != null)
            {
                for (int i = 0; i < _darkUIImages.Count; i++)
                {
                    if (_darkUIImages[i] != null)
                        _darkUIImages[i].color = _player.ClassData.CharacterUIColorDark;
                }
            }
        }
    }

    #endregion
}
