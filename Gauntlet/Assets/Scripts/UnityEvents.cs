using UnityEngine.Events;

//runs when a player joins or leaves, used primarily for updating UI
public class PlayersChanged : UnityEvent<Player[]> { }

//runs when a player dies
public class PlayerDied : UnityEvent<Player[]> { }

//runs when a player clears the level
public class PlayerClearedLevel: UnityEvent<Player[]> { }

//runs when available classes are determined
public class AvailableClassesUpdated: UnityEvent<ClassData[]> { }

//runs when trying to add a player
public class TryAddPlayer: UnityEvent<int, ClassData> { }
