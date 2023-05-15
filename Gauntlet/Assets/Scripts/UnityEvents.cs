using UnityEngine.Events;

//runs when a player joins or leaves, used primarily for updating UI
public class PlayersChanged : UnityEvent<Player[]> { }

