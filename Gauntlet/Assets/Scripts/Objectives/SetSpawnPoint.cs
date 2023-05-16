using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
   private void OnEnable()
    {
        SetPlayerSpawn();
    }

    public void SetPlayerSpawn()
    {
        LevelManager.playerSpawn = transform;
    }
}
