using UnityEngine;

//a partial facade for the projectiles
//uses:
//1. Simple Rotation Script (should be added on the prefab)
//2. Projectile Fade Animator (may or maybot be manualy attatched to the prefab

//handles collisions and updates aforementioned scripts 

public class Projectile : MonoBehaviour
{
    [Tooltip("Additional spawn offset for the projectile relative to what created it. Y is Forward/Backward, X is Left/Right")][SerializeField] private Vector2 _spawnOffset = Vector2.zero;

    //projectile data
    private GameObject _sourceEntity;

    private ProjectileData _projectileData;


    //projectile lifetime control
    private float _creationTime;
    private readonly int _destroyTime = 10;

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

        //gets current time
        _creationTime = Time.time;
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
            if (collision.gameObject.TryGetComponent(out IGameEntity gameEntity))
            {
                if (gameEntity.EntityType != _projectileData.projectileSourceType)
                {
                    gameEntity.ReactToShot(_projectileData.ProjectileDamage, gameObject);
                }
            }

            ClearProjectile();
        }
        else
        {
            if(collision.gameObject != _sourceEntity)
            {
                if (collision.gameObject.TryGetComponent(out IGameEntity gameEntity))
                {
                    if (gameEntity.EntityType != _projectileData.projectileSourceType)
                    {
                        gameEntity.ReactToShot(_projectileData.ProjectileDamage, _sourceEntity);
                    }
                }

                ClearProjectile();
            }
        }

    }
    //Clears projectile if its alive too long
    private void Update()
    {
        //tracks projectile lifetime and sends back to pool after its around for way to long (10 Seconds)
        if(_creationTime + _destroyTime < Time.time)
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
    public void InitilizeProjectile(GameObject sourceEntity, int damage, ProjectileSourceType type)
    {
        //initilizes projectile information
        _sourceEntity = sourceEntity;
        _projectileData.SetProjectileData(damage, type);

        //adds the projectile specific offset (assumes what launched the projectile spawned the projectile at its position)
        if(sourceEntity != null)
        {
            transform.position += sourceEntity.transform.forward * _spawnOffset.y;
            transform.position += sourceEntity.transform.right * _spawnOffset.x;
        }

    }

    #endregion
}