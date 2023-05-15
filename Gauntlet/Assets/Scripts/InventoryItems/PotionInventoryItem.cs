using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionInventoryItem : IInventoryItem
{
    //set on the input in PlayerControls
    static public int _potionDamage;
    static public GameObject _sourceEntity;

    public void UseItem()
    {
        ApplyPotionDamage();
        Debug.Log("Potion Used!");
    }

    private void ApplyPotionDamage()
    {
        Death deathEnemy;
        int visibleEnemyCount = 0;
        foreach (EnemyParent enemy in GameObject.FindObjectsOfType<EnemyParent>())
        {
            Debug.Log("Enemies in Scene: " + GameObject.FindObjectsOfType<EnemyParent>().Length);
            if (enemy.isVisible)
            {
                visibleEnemyCount++;
                enemy.ReactToShot(_potionDamage, _sourceEntity);
                if (enemy.TryGetComponent<Death>(out deathEnemy))
                    enemy.OnDeath();
            }
        }

        Debug.Log("Eneies Visible: " + visibleEnemyCount);

        foreach(Spawner spawner in GameObject.FindObjectsOfType<Spawner>())
        {
            if (spawner.isVisible)
            {
                spawner.ReactToShot(_potionDamage, _sourceEntity);
            }
        }
    }
}
