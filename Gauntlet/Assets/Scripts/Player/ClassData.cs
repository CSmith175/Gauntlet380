using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "GauntletData/ClassData", order = 1)]
public class ClassData : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject _classCharacterPrefab;

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    public GameObject SpawnClassPrefab()
    {
        if (_classCharacterPrefab)
        {
            return Instantiate(_classCharacterPrefab);
        }
        else
        {
            Debug.LogWarning("No class prefab found on class data for: <b>" + name + "<b> A cube was spawned as a placeholder");
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }
}
