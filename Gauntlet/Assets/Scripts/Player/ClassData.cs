using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "GauntletData/ClassData", order = 1)]
public class ClassData : ScriptableObject
{
    #region "Stat Inspector Variables"
    [SerializeField] private int _baseMoveSpeed;
    [SerializeField] private int _baseShotAttack;
    [SerializeField] private int _baseMeleeAttack;
    [SerializeField] private int _baseMagicAttack;
    [SerializeField] private int _baseDefense;
    [SerializeField] private int _baseShotSpeed;
    [SerializeField] private int _baseHealth;
    [SerializeField] private GameObject _characterModelPrefab;
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
