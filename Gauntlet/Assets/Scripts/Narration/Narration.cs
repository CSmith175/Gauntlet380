using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Narration
{
    public readonly string narrationName; //name of the narration, used to help the editor overwrite already exsisting ones instead of creating new ones, and to keep organized. used to acsess from Json through dictionary.
    public readonly NarrationType narrationType; // the type of narration, used to sort out through all the narrations

    public readonly NarrationInputParing[] narrationInputPairings; //a paring of a data type and a string. used to slide in the appropiate data into the structure strings
    //the string inside the saved one is used as the default display data, though it is overidded if the event suplys data that can overite it when it is called. 

    public readonly string[] narrationStructureStrings; //strings that control each chuck of text, inbetween these context sisitive data is injected like player names.


    //constructor used when creating through the editor script
    public Narration(string newNarrationName, NarrationType newNarrationType, NarrationInputParing[] newNarrationInputPairings, string[] newNarrationStructureStrings)
    {
        narrationName = newNarrationName;
        narrationType = newNarrationType;
        narrationInputPairings = newNarrationInputPairings;
        narrationStructureStrings = newNarrationStructureStrings;
    }

    //constructor that converts from NarrationSeriazed to a narration. needed for easily loading back a naration from the database
    public Narration (NarrationSerilizable loadedNarration) 
    {
        narrationName = loadedNarration.narrationName; //Stays the Same
        GetNarrationTypeEnumFromString(loadedNarration, out narrationType); //gets theh enum back out from the saved versions strring formatting

        CreateNarrationInputPairingsFromStringArrays(loadedNarration, out narrationInputPairings); //creates the input parings from the loaded data

        narrationStructureStrings = loadedNarration.narrationStructureStrings; //sets the structure strings back (stays the same)
    }

    //functions that are used in the conversion process
    #region "Conversion Functions"
    private void GetNarrationTypeEnumFromString(NarrationSerilizable loadedNarration, out NarrationType narrationType)
    {
        foreach(string nString in Enum.GetNames(typeof (NarrationType)))
        {
            if(nString == loadedNarration.narrationTypeString)
            {
                narrationType = (NarrationType)Enum.Parse(typeof(NarrationType), nString);
                return;
            }
        }

        narrationType = NarrationType.Undefined;
    }

    private void CreateNarrationInputPairingsFromStringArrays(NarrationSerilizable loadedNarration, out NarrationInputParing[] loadedPairings)
    {
        loadedPairings = new NarrationInputParing[loadedNarration.narrationInformationTypeStrings.Length];

        bool pairingset;
        for (int i = 0; i < loadedPairings.Length; i++)
        {
            pairingset = false;

            foreach (string nString in Enum.GetNames(typeof(NarrationInformationType)))
            {
                if (nString == loadedNarration.narrationInformationTypeStrings[i])
                {
                    pairingset = true;
                    loadedPairings[i].informationType = (NarrationInformationType)Enum.Parse(typeof(NarrationInformationType), nString);
                }
            }

            if (!pairingset)
                loadedPairings[i].informationType = NarrationInformationType.NoType;

            loadedPairings[i].narrationString = loadedNarration.narrationInputStrings[i];
        }
    }
    #endregion

} //Naration in a usable form, created through the editor and the JSON converter

[System.Serializable]
public class NarrationSerilizable //Narration broken down to be serialized to JSON. Conversion happens in the constructor.
{
    public string narrationName; //name (unchanged)
    public string narrationTypeString; //the narration type enum as it's name

    //Narration Input Paring broken down
    public string[] narrationInformationTypeStrings; //information type enum as a string component as an array
    public string[] narrationInputStrings; //inputted naration strings as an array (unchanged) (functions as the default display too)

    public string[] narrationStructureStrings; //narration strings (unchanged)

    public NarrationSerilizable(Narration narration) //constructor, converts a narration into this savable format
    {
        narrationName = narration.narrationName; //sets the name
        narrationTypeString = Enum.GetName(typeof(NarrationType), narration.narrationType); //sets the string by converting enum to string through System

        BreakdownInputParings(out narrationInformationTypeStrings, out narrationInputStrings, narration);

        narrationStructureStrings = narration.narrationStructureStrings;
    }

    //breaks down and outputs the InputParings to string[]s
    private void BreakdownInputParings(out string[] informationTypeStrings, out string[] informationContentStrings, Narration narration)
    {
        informationTypeStrings = new string[narration.narrationInputPairings.Length];
        informationContentStrings = new string[narration.narrationInputPairings.Length];

        for (int i = 0; i < narration.narrationInputPairings.Length; i++)
        {
            informationTypeStrings[i] = Enum.GetName(typeof(NarrationInformationType), narration.narrationInputPairings[i].informationType); //converts the narrationinformation enum to a string
            informationContentStrings[i] = narration.narrationInputPairings[i].narrationString; 
        }
    }

}


[System.Serializable]
public class NarrationSerilizableContainer //Container for all of the narration serilizables. sorts and unsorts a dictionary based on type. dictionarys cant be directly saved so this is neccesary
{
    public List<NarrationSerilizable> savedNarrations = new List<NarrationSerilizable>();

    public List<string> narrationTypeNames = new List<string>();
    public List<int> narrationsTypeIndexing = new List<int>();

    public NarrationSerilizableContainer(Dictionary<NarrationType, List<Narration>> gameFormattedNarrationDictionary)
    {
        List<Narration> currentNarrationTypeList;
        int indexIncrease;

        foreach (int value in Enum.GetValues(typeof(NarrationType)))
        {
            if(gameFormattedNarrationDictionary.TryGetValue((NarrationType)value, out currentNarrationTypeList)) //gets the list from the category, as unserilizable narrations.
            {
                indexIncrease = 0;
                for (int i = 0; i < currentNarrationTypeList.Count; i++) //adds to content list
                {
                    savedNarrations.Add(new NarrationSerilizable(currentNarrationTypeList[i])); //converts narration to savable format and adds to list
                    indexIncrease++;
                }
                //handles tracking of formatting
                narrationTypeNames.Add(Enum.GetName(typeof(NarrationType), (NarrationType)value)); //adds the enum name from the value
                narrationsTypeIndexing.Add(indexIncrease); //adds the index that later read to translate the list back into its dictionary form (dictionaries are just fancy 2D arrays)
            }
        }





    } //converts game dictionary to serilizable list of NarrationSerilizable, along with information on the narration types and indexing to be able to convert it back

    //takes the serilized list format and converts back to game usable
    public Dictionary<NarrationType, List<Narration>> GetValuesAsGameUsable()
    {
        Dictionary<NarrationType, List<Narration>> loadedGameReadyDictionary = new Dictionary<NarrationType, List<Narration>>();

        if (savedNarrations != null && narrationTypeNames != null && narrationsTypeIndexing != null)
        {
            int mainIndexer = 0; //used for tracking overall movement through saved narrations
            int subIndexer = 0; //used for determining what sublist is being worked on
            //type names and type indexing will always have the same length
            for (int i = 0; i < narrationsTypeIndexing.Count; i++)
            {
                List<Narration> currentNarrationList = new List<Narration>();

                subIndexer = 0;

                string typeString = narrationTypeNames[i];

                NarrationType currentType = (NarrationType)Enum.Parse(typeof(NarrationType), typeString);


                while (subIndexer < narrationsTypeIndexing[i])
                {

                    currentNarrationList.Add(new Narration(savedNarrations[subIndexer + mainIndexer]));
                    subIndexer++;
                }

                loadedGameReadyDictionary.Add(currentType, currentNarrationList);
                mainIndexer += narrationsTypeIndexing[i];
            }

            return loadedGameReadyDictionary;
        }
        else
        {
            //null values (should be impossible due to the constructor) returns empty
            return loadedGameReadyDictionary;
        }    
    }

}