using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPickup : MonoBehaviour
{
    public PlayerStatCategories statPowerUp;
    public int powerUpAmount;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            Player player;
            other.TryGetComponent<Player>(out player);
            if(player != null)
            {
                player.PlayerStats.IncrementPlayerStat(statPowerUp, powerUpAmount);
                gameObject.SetActive(false);
            }
        }
    }
}
