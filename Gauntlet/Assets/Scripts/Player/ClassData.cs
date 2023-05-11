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
    [SerializeField] private int _baseMoveSpeed;
    [SerializeField] private int _baseShotAttack;
    [SerializeField] private int _baseMeleeAttack;
    [SerializeField] private int _baseMagicAttack;
    [SerializeField] private int _baseDefense;
    [SerializeField] private int _baseShotSpeed;
    [SerializeField] private int _baseHealth;


    [Header("Player Art Prefabs")]
    [Space(15)]
    [SerializeField] private GameObject _characterModelPrefab;
    [SerializeField] private GameObject _characterShotPrefab;
    #endregion

    #region "Stat Getter Properties"
    public int BaseMoveSpeed { get { return _baseMoveSpeed; } }
    public int BaseShotAttack { get { return _baseShotAttack; } }
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
