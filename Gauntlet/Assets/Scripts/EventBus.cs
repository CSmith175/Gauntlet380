public static class EventBus
{

    //event for when the players in the game change (a player joins or drops)
    private static PlayersChanged _playerCountChangedEvent = new PlayersChanged();
    public static PlayersChanged OnPlayerChanged { get { return _playerCountChangedEvent; } }


}
