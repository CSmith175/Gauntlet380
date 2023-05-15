using UnityEngine;

public class PickupLogic : MonoBehaviour
{
    public ItemType item;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (player.PlayerInventory.CheckIfInventoryFull())
                {
                    return;
                }
                else
                {
                    player.PlayerInventory.TryAddItem(item);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
