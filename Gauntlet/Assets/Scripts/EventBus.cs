using System;

public static class EventBus
{
    public delegate void Soup();

    //event for when the players in the game change (a player joins or drops)
    private static PlayersChanged _playerCountChangedEvent = new PlayersChanged();
    public static PlayersChanged OnPlayerChanged { get { return _playerCountChangedEvent; } }

    #region "Player Death Events
    //one player died
    private static PlayerDied _playerDiedEvent = new PlayerDied();
    public static PlayerDied OnPlayerDied { get { return _playerDiedEvent; } }

    //for when all players are dead
    public static Action _allPlayersDeadEvent;
    public static Action OnAllPlayersDead { get { return _allPlayersDeadEvent; } }
    #endregion

    #region "Player Clear Events"
    //one player clear
    private static PlayerClearedLevel _playerClearEvent = new PlayerClearedLevel();
    public static PlayerClearedLevel OnPlayerClear { get { return _playerClearEvent; } }

    //all Players Clear
    public static Action _allPlayersClearEvent;
    public static Action OnAllPlayersClear { get { return _allPlayersClearEvent; } }
    #endregion

    //event for when the available classes for player's selection are updated
    private static AvailableClassesUpdated _availableClassesUpdatedEvent = new AvailableClassesUpdated();
    public static AvailableClassesUpdated OnAvailableClassesUpdated { get { return _availableClassesUpdatedEvent; } }

    //for when trying to add a player with a specific controller ID and class
    private static TryAddPlayer _tryAddPlayerEvent = new TryAddPlayer();
    public static TryAddPlayer OnTryAddPlayer { get { return _tryAddPlayerEvent; } }
}
