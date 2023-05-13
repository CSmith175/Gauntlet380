using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "GauntletData/ClassData", order = 1)]
public class ClassData : ScriptableObject
{
    #region Display Inspector Variables
    [Header("Display Info")]
    [SerializeField] private string _characterName;
    #endregion

    #region "Display Getter Properties"
    public string CharacterName { get { return _characterName; } }
    #endregion

    #region "Stat Inspector Variables"
    [Header("Stats")]
    [Space(15)]
    [Tooltip("Movement speed of the player.")] [SerializeField] private int _baseMoveSpeed;
    [Tooltip("Damage the player does with their projectile attack.")] [SerializeField] private int _baseShotAttack;
    [Tooltip("Time in seconds players need to recharge between shots.")] [SerializeField] private float _baseShotDowntime;
    [Tooltip("Damage the player does with their melee attack.")] [SerializeField] private int _baseMeleeAttack;
    [Tooltip("Damage the player does with their magic attack (Potions).")] [SerializeField] private int _baseMagicAttack;
    [Tooltip("Damage Reduction the player receives when getting hit.")] [SerializeField] private int _baseDefense;
    [Tooltip("Movement Speed of the player's Projectiiles.")] [SerializeField] private int _baseShotSpeed;
    [Tooltip("Starting health of the player.")] [SerializeField] private int _baseHealth;


    [Header("Player Art Prefabs")]
    [Space(15)]
    [Tooltip("Prefab of the character model, containing anything needed for visual display.")] [SerializeField] private GameObject _characterModelPrefab;
    [Tooltip("Prefab for the projectile, containing anything needed for visual display and the Projectile.cs script.")] [SerializeField] private GameObject _characterShotPrefab;
    #endregion

    #region "Stat Getter Properties"
    public int BaseMoveSpeed { get { return _baseMoveSpeed; } }
    public int BaseShotAttack { get { return _baseShotAttack; } }
    public float BaseShotDowntime { get { return _baseShotDowntime; } }
    public int BaseMeleeAttack { get { return _baseMeleeAttack; } }
    public int BaseMagicAttack { get { return _baseMagicAttack; } }
    public int BaseDefense { get { return _baseDefense; } }
    public int BaseShotSpeed { get { return _baseShotSpeed; } }
    public int BaseHealth { get { return _baseHealth; } }
    #endregion

    #region "Art Getter Properties
    public GameObject CharacterShotPrefab { get { return _characterShotPrefab; } }
    #endregion

    public GameObject SpawnClassPrefab()
    {
        if (_characterModelPrefab)
        {
            return Instantiate(_characterModelPrefab);
        }
        else
        {
            Debug.LogWarning("No class prefab found on class data for: <b>" + name + "<b> A cube was spawned as a placeholder");
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }


}
