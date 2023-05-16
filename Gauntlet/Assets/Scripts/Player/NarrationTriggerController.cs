using System.Collections.Generic;
using System;

//attatches to players, stores information pairings related to a player
public class NarrationTriggerController
{
    private readonly Dictionary<NarrationInformationType, NarrationInputParing> _trackedPairings = new Dictionary<NarrationInformationType, NarrationInputParing>();
    private readonly List<NarrationInputParing> _trackedPairingList = new List<NarrationInputParing>();

    private NarrationInputParing _currentPairing;


    public void TriggerNarration(NarrationType type)
    {
        if(_trackedPairingList != null)
        {
            NarrationManager.TriggerNarration(type, _trackedPairingList.ToArray());
        }
        else
        {
            NarrationManager.TriggerNarration(type, new NarrationInputParing[0]);
        }

        ClearPairings();
    }


    #region "Pairing Managment Functions

    // Updatesinformation to the pairing of the given type, used in the adding adding functions
    private void UpdatePairing(NarrationInformationType type, NarrationInputParing pairing)
    {
        if (_trackedPairings.ContainsKey(type))
        {
            _trackedPairings.TryGetValue(type, out _currentPairing);
            if (_trackedPairingList.Contains(_currentPairing))
            {
                _trackedPairingList.Remove(_currentPairing);
            }
        }
        _trackedPairings.Remove(type);

        _trackedPairings.Add(type, pairing);
        _trackedPairingList.Add(pairing);
    }

    //removes all pairings from the dictionary and list, ran after a narration is triggered
    private void ClearPairings()
    {
        _trackedPairings.Clear();
        _trackedPairingList.Clear();
    }

    #endregion

    #region "Pairing Adding Functions;

    /// <summary>
    /// Add a pairing for an enemy name
    /// </summary>
    /// <param name="data"> players class data </param>
    /// <returns></returns>
    public NarrationInputParing CreatePlayerNamePairing(ClassData data)
    {
        NarrationInputParing pairing;
        pairing.informationType = NarrationInformationType.PlayerClassName;
        pairing.narrationString = data.CharacterName;

        UpdatePairing(pairing.informationType, pairing);
        return pairing;
    }

    /// <summary>
    /// Add a pairing for an enemy name
    /// </summary>
    /// <param name="stats"> enemys stats </param>
    /// <returns></returns>
    public NarrationInputParing CreateEnemyNamePairing(EnemyStats stats)
    {
        NarrationInputParing pairing;
        pairing.informationType = NarrationInformationType.EnemyName;
        pairing.narrationString = stats.enemyName;


        UpdatePairing(pairing.informationType, pairing);
        return pairing;
    }

    /// <summary>
    /// Add a pairing for a food name
    /// </summary>
    /// <param name="foodName"> name of the food item </param>
    /// <returns></returns>
    public NarrationInputParing CreateFoodPairing(string foodName)
    {
        NarrationInputParing pairing;
        pairing.informationType = NarrationInformationType.FoodName;
        pairing.narrationString = foodName;


        UpdatePairing(pairing.informationType, pairing);
        return pairing;
    }

    /// <summary>
    /// Add a pairing for a damage value inflicted to a player
    /// </summary>
    /// <param name="damageValue"> integer value of the damage (positive) </param>
    /// <returns></returns>
    public NarrationInputParing CreateDamageValuePairing(int damageValue)
    {
        NarrationInputParing pairing;
        pairing.informationType = NarrationInformationType.DamageValue;
        pairing.narrationString = damageValue.ToString();


        UpdatePairing(pairing.informationType, pairing);
        return pairing;
    }

    /// <summary>
    /// Add a pairing for a heal value on a player
    /// </summary>
    /// <param name="healValue"> integer value of the health gained </param>
    /// <returns></returns>
    public NarrationInputParing CreateHealValuePairing(int healValue)
    {
        NarrationInputParing pairing;
        pairing.informationType = NarrationInformationType.HealValue;
        pairing.narrationString = healValue.ToString();


        UpdatePairing(pairing.informationType, pairing);
        return pairing;
    }

    /// <summary>
    /// Add a pairing for a treausre value gained by the player
    /// </summary>
    /// <param name="treasureValue"> score value of the treasure </param>
    /// <returns></returns>
    public NarrationInputParing CreateTreasureValuePairing(int treasureValue)
    {
        NarrationInputParing pairing;
        pairing.informationType = NarrationInformationType.TreasureValue;
        pairing.narrationString = treasureValue.ToString();


        UpdatePairing(pairing.informationType, pairing);
        return pairing;
    }

    /// <summary>
    /// Add a pairing for a stat category on a player
    /// </summary>
    /// <param name="statCategory"> category of the stat in question </param>
    /// <returns></returns>
    public NarrationInputParing CreateStatPairing(PlayerStatCategories statCategory)
    {
        NarrationInputParing pairing;
        pairing.informationType = NarrationInformationType.Stat;
        pairing.narrationString = Enum.GetName(typeof(PlayerStatCategories), statCategory);


        UpdatePairing(pairing.informationType, pairing);
        return pairing;
    }

    #endregion
}
