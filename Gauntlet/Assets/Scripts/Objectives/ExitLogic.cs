using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Player player;
            Debug.Log("Player Exited");
            other.gameObject.TryGetComponent<Player>(out player);

            if(player != null)
            {
                EventBus.OnPlayerClear?.Invoke(player);
            }
        }
    }
}
