using UnityEngine;

//a simple rotation script that rotates an object. for visuals on stuff like projectiles
public class SimpleRotationScript : MonoBehaviour
{
    #region "Variables"
    [Header("Use Random Rotation?")]
    [SerializeField] private bool _randomRotation;
    [Header("Stop Rotation On Collisions?")]
    [Space(5)]
    [SerializeField] private bool _stopOnCollision;


    //for random rotation
    [Header("Random rotation parameters")]
    [Space(10)]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _minSpeed;

    //for manual rotation
    [Header("Manualy Set Rotation Vector")]
    [Space(10)]
    [SerializeField] private Vector3 _manualRotationVector;

    //applied movement vector
    private Vector3 _appliedRotationVector = new Vector3();
    #endregion

    //sets up applied rotation vector
    private void OnEnable()
    {
        if(_randomRotation)
        {
            _appliedRotationVector.x = Random.Range(_minSpeed, _maxSpeed);
            _appliedRotationVector.y = Random.Range(_minSpeed, _maxSpeed);
            _appliedRotationVector.z = Random.Range(_minSpeed, _maxSpeed);
        }
        else
        {
            _appliedRotationVector = _manualRotationVector;
        }
    }

    //rotates object, in late update to not interfere with projectile pathing
    private void LateUpdate()
    {
        transform.Rotate(_appliedRotationVector * Time.deltaTime);
    }

    /// <summary>
    /// Stops the rotation until the object is enabled again if stop rotation on collision is enabled
    /// </summary>
    public void StopRotationFromCollision()
    {
        if(_stopOnCollision)
            _appliedRotationVector = Vector3.zero;
    }

}
