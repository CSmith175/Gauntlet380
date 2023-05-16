using UnityEngine;

public class FoodLogic : MonoBehaviour
{
    public string foodName = "Borger";
    public int healthAddValue = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if(other.TryGetComponent(out Player player))
            {
                player.PlayerStats.IncrementPlayerStat(PlayerStatCategories.Health, healthAddValue);

                //sets up and triggers narration
                if(player.NarrationController != null)
                {
                    player.NarrationController.CreateFoodPairing(foodName);
                    player.NarrationController.CreateHealValuePairing(healthAddValue);
                    player.NarrationController.CreatePlayerNamePairing(player.ClassData);
                    player.NarrationController.TriggerNarration(NarrationType.FoodPickedUp);
                }

                gameObject.SetActive(false);
            }
        }
    }
}
