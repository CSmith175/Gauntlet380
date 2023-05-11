using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLogic : MonoBehaviour
{
    public int healthAddValue = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            other.GetComponent<Player>().PlayerStats.IncrementPlayerStat(PlayerStatCategories.Health, healthAddValue);
            gameObject.SetActive(false);
        }
    }
}
