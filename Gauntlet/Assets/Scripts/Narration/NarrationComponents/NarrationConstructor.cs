using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationConstructor
{
    //filepath, should be the exact same as the one on the editor
    private string _jsonFilePath = "Assets/Resources/Narrations/NarrationDatabase.json";

    //1. gets random narration of given type
    //2. sorts inputs
    //3. injects into base narration
    //4. converts to string
    //null check by checking for an empty string ("")

    public string CreateNarationStringFromEvent(NarrationType type, NarrationInputParing[] inputs)
    {
        if(TryGetRandomNarrationsOfType(type, out Narration baseNarration))
        {
            Narration injectedNarration = NarrationInformationInjection(baseNarration, SortInputParings(inputs));
            return ConvertNarrationToString(injectedNarration);
        }
        else
        {
            return "";
        }
    }



    //gets a random narration from the JSON file of the give type
    private bool TryGetRandomNarrationsOfType(NarrationType type, out Narration narration)
    {
        NarrationJsonConverter jsonRetreiver = new NarrationJsonConverter(_jsonFilePath);

        Dictionary<NarrationType, List<Narration>> retreivedNarrations = jsonRetreiver.GetNarrationsAsGameFormat();
        List<Narration> listOfType;

        if (retreivedNarrations != null)
        {
            if (retreivedNarrations.TryGetValue(type, out listOfType))
            {
                int selection = Random.Range(0, listOfType.Count);
                narration = listOfType[selection];
                return true;
            }
            else
            {
                Debug.LogWarning("No Narrations in the inputted type.");
            }
        }
        else
        {
            Debug.LogError("Couldn't load Json in NarrationConstructor Class.");
        }
        narration = null;
        return false;
    }

    //formats the inputted array into a dictionary of Queues. the type acts as a key and the strings are then added to a Queue to maintain order
    private Dictionary<NarrationInformationType, Queue<string>> SortInputParings(NarrationInputParing[] inputs)
    {
        Dictionary<NarrationInformationType, Queue<string>> sortedParings = new Dictionary<NarrationInformationType, Queue<string>>();
        Queue<string> currentStringQueue;

        for (int i = 0; i < inputs.Length; i++)
        {
            if (sortedParings.TryGetValue(inputs[i].informationType, out currentStringQueue))
            {
                currentStringQueue.Enqueue(inputs[i].narrationString);
            }
            else
            {
                currentStringQueue = new Queue<string>();
                currentStringQueue.Enqueue(inputs[i].narrationString);

                sortedParings.Add(inputs[i].informationType, currentStringQueue);
            }
        }

        return sortedParings;
    }

    //injects the parings into the base narration. returns out a new narration with the injected contents.
    private Narration NarrationInformationInjection(Narration baseNarration, Dictionary<NarrationInformationType, Queue<string>> sortedPairings)
    {
        NarrationInputParing[] pairingsToInject = baseNarration.narrationInputPairings;
        Queue<string> currentQueue;

        //sets pairing strings
        for (int i = 0; i < pairingsToInject.Length; i++)
        {
            if(sortedPairings.TryGetValue(pairingsToInject[i].informationType, out currentQueue))
            {
                if(currentQueue != null)
                {
                    if(currentQueue.Count > 0)
                    {
                        //sets it to the first string in the queue
                        pairingsToInject[i].narrationString = currentQueue.Dequeue();
                    }
                }
            }
        }

        //returns out the new narration with the modified parings
        return new Narration(baseNarration.narrationName, baseNarration.narrationType, pairingsToInject, baseNarration.narrationStructureStrings);
    }

    //converts the narration to one cacatonated string
    private string ConvertNarrationToString(Narration narrationToString)
    {
        string outputString = "";

        for (int i = 0; i < narrationToString.narrationStructureStrings.Length; i++)
        {
            outputString += narrationToString.narrationStructureStrings[i];

            if(narrationToString.narrationInputPairings.Length > i)
            {
                outputString += narrationToString.narrationInputPairings[i].narrationString;
            }
        }

        return outputString;
    }
}
