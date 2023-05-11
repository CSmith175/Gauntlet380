using UnityEngine;

//a partial facade for the projectiles
//uses:
//1. Simple Rotation Script (should be added on the prefab)
//2. Projectile Fade Animator (may or maybot be manualy attatched to the prefab

//handles collisions and updates aforementioned scripts 

public class Projectile : MonoBehaviour
{
    private GameObject _sourceEntity;

    #region "Component Variables"
    private ProjectileFadeAnimator _fadeAnimator; //can be set up on prefab, dosen't matter either way. 
    private SimpleRotationScript _simpleRotationScript; //should be set up on prefab

    private Rigidbody _projectileRBody; //can be set up here, should be set up on prefab
    private Collider _projectileCollider; //must be set up on prefab
    #endregion

    #region "Unity Functions"
    //sets up projectile when gotten from the pool
    private void OnEnable()
    {
        //sets up fade animator
        if(_fadeAnimator == null)
        {
            TryGetComponent(out _fadeAnimator);

            if(_fadeAnimator == null)
            {
                _fadeAnimator = gameObject.AddComponent<ProjectileFadeAnimator>();
            }
        }
        _fadeAnimator.ResetFade(); //resets possible previous fade from Fade Animator                                  
        _fadeAnimator.fadeComplete += DisableProjectile; //subscribes projectile disable function to the end of the fade out

        //sets up simple rotation
        if (_simpleRotationScript == null)
        {
            TryGetComponent(out _simpleRotationScript);

            if (_simpleRotationScript == null)
            {
                _simpleRotationScript = gameObject.AddComponent<SimpleRotationScript>();
            }
        }

        //sets up rigid body
        if (_projectileRBody == null)
        {
            TryGetComponent(out _projectileRBody);

            if(_projectileRBody == null)
            {
                _projectileRBody = gameObject.AddComponent<Rigidbody>();
            }
        }
        ResetRigidbody(); //unlocks the movement if it was locked previously, and sets up fresh rigidbodies

        //sets up collider
        if (_projectileCollider == null)
        {
            gameObject.TryGetComponent(out _projectileCollider);       
        }
        if (_projectileCollider) _projectileCollider.enabled = true; //re-enables the collider
    }
    //removes event subscription to prevent errors
    private void OnDisable()
    {
        _fadeAnimator.fadeComplete -= DisableProjectile;
    }
    //checks collision
    private void OnCollisionEnter(Collision collision)
    {
        if (!_sourceEntity)
        {
            ClearProjectile();
        }
        else if (collision.gameObject != _sourceEntity)
        {
            ClearProjectile();
        }
    }
    #endregion

    #region "Local Functions"
    /// <summary>
    /// Locks the projectile in place, for when it collides with a wall
    /// </summary>
    private void LockProjectileInPlace()
    {
        if (_projectileRBody)
        {
            _projectileRBody.velocity = Vector3.zero;
            _projectileRBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    /// <summary>
    /// Creates a new rigidbody
    /// </summary>
    private void ResetRigidbody()
    {
        _projectileRBody.useGravity = false;

        _projectileRBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
    /// <summary>
    /// disables the projectile, sendning it back to the shadow realm (its right by the chick-fil-a)
    /// </summary>
    private void DisableProjectile()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Begins the process of clearing the projectile. happens on collision or if the rigidbody falls asleep
    /// </summary>
    private void ClearProjectile()
    {
        //stops the projectile's movement and begin's its fading process
        LockProjectileInPlace();
        _fadeAnimator.FadeProjectile(2);
        _simpleRotationScript.StopRotationFromCollision();
        if (_projectileCollider) _projectileCollider.enabled = false;
    }
    #endregion

    #region "Public Initilization Functions"
    public void SetSourceEntity(GameObject sourceEntity)
    {
        _sourceEntity = sourceEntity;
    }

    #endregion
}