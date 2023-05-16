using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NarrationDebugger : MonoBehaviour
{
    private NarrationInputParing[] debuggingPairings;

    /* Default inputs in the debugger are initilized to these values for testing
     *    NoType,            MissingNO, The Concept of Time     
          PlayerClassName,    Questor, Thor  
          EnemyName,          Death, Lobber
          FoodName,           Hamborger, Shamke
          DamageValue,        7, 13
          HealValue,         30, 120
          TreasureValue,     777, 310
     * */

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, Screen.height / 2, 400, Screen.height / 2)); 
        GUILayout.BeginVertical();
        foreach (int value in Enum.GetValues(typeof(NarrationType)))
        {
            if(GUILayout.Button("Random Event of type: " + Enum.GetName(typeof(NarrationType), (NarrationType)value)))
            {
                InitilizeDebugPairingsFull();
                NarrationManager.TriggerNarration((NarrationType)value, debuggingPairings);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    //initilized 2 into each
    private void InitilizeDebugPairingsFull()
    {
        debuggingPairings = new NarrationInputParing[14];

        //yes this was tedious
        debuggingPairings[0] = SetUpDebugPairing(NarrationInformationType.NoType, "MissingNO");
        debuggingPairings[1] = SetUpDebugPairing(NarrationInformationType.NoType, "The Concept of Time");

        debuggingPairings[2] = SetUpDebugPairing(NarrationInformationType.PlayerClassName, "Questor");
        debuggingPairings[3] = SetUpDebugPairing(NarrationInformationType.PlayerClassName, "Thor");

        debuggingPairings[4] = SetUpDebugPairing(NarrationInformationType.EnemyName, "Death");
        debuggingPairings[5] = SetUpDebugPairing(NarrationInformationType.EnemyName, "Lobber");

        debuggingPairings[6] = SetUpDebugPairing(NarrationInformationType.FoodName, "Hamborger");
        debuggingPairings[7] = SetUpDebugPairing(NarrationInformationType.FoodName, "Shamke");

        debuggingPairings[8] = SetUpDebugPairing(NarrationInformationType.DamageValue, "7");
        debuggingPairings[9] = SetUpDebugPairing(NarrationInformationType.DamageValue, "13");

        debuggingPairings[10] = SetUpDebugPairing(NarrationInformationType.HealValue, "30");
        debuggingPairings[11] = SetUpDebugPairing(NarrationInformationType.HealValue, "120");

        debuggingPairings[12] = SetUpDebugPairing(NarrationInformationType.TreasureValue, "777");
        debuggingPairings[13] = SetUpDebugPairing(NarrationInformationType.TreasureValue, "310");
    }
    //initilizes player field to one, and some to empty. 
    private void InitilizeDebugPairingsSemi()
    {
        debuggingPairings = new NarrationInputParing[9];

        //yes this was tedious
        debuggingPairings[0] = SetUpDebugPairing(NarrationInformationType.NoType, "MissingNO");
        debuggingPairings[1] = SetUpDebugPairing(NarrationInformationType.NoType, "The Concept of Time");

        debuggingPairings[2] = SetUpDebugPairing(NarrationInformationType.PlayerClassName, "Questor");


        debuggingPairings[3] = SetUpDebugPairing(NarrationInformationType.DamageValue, "7");
        debuggingPairings[4] = SetUpDebugPairing(NarrationInformationType.DamageValue, "13");

        debuggingPairings[5] = SetUpDebugPairing(NarrationInformationType.HealValue, "30");
        debuggingPairings[6] = SetUpDebugPairing(NarrationInformationType.HealValue, "120");

        debuggingPairings[7] = SetUpDebugPairing(NarrationInformationType.TreasureValue, "777");
        debuggingPairings[8] = SetUpDebugPairing(NarrationInformationType.TreasureValue, "310");
    }
    //initilizes empty pairings
    private void InitilizeEmptyPairings()
    {
        debuggingPairings = new NarrationInputParing[0];
    }

    private NarrationInputParing SetUpDebugPairing(NarrationInformationType type, string text)
    {
        NarrationInputParing pairing;
        pairing.informationType = type;
        pairing.narrationString = text;
        return pairing;
    }
}
