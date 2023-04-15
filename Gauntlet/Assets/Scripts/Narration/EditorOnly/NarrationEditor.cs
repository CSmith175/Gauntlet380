using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

#if UNITY_EDITOR
using UnityEditor;
public class NarrationEditor : EditorWindow
{
    //JsonManager
    private string _jsonFilePath = "Assets/Resources/Narrations/NarrationDatabase.json";

    //viewing
    private Dictionary<NarrationType, List<Narration>> narrationDictionary;

    #region "Variables"
    //new narration
    private string _newNarrationName;
    private NarrationType _newNarrationType;
    private string[] _newNarrationStrings = new string[2];
    private NarrationInputParing[] _newNarrationInformationPairing;
    private string _narrationPreviewString;

    //editor
    private static EditorWindow _narrationEditorWindowInstance;

    private Vector2 _dictionaryScroll = Vector2.zero;
    #endregion

    [MenuItem("Narration/NarrationEditor")]
    public static void ShowNarrationEditorWindow()
    {
        _narrationEditorWindowInstance = GetWindow<NarrationEditor>("Narration Asset Editor");
        _narrationEditorWindowInstance.minSize = new Vector2Int(600, 300);
    }

    private void OnGUI()
    {
        DrawViewAllNarrationsWindow();
    }

    //Narrations
    private void DrawNewNarrationWindow() //draws the window for creating new narrations. 
    {
        GUIStyle textWrapStyle = new GUIStyle(GUI.skin.GetStyle("label"))
        {
            wordWrap = true
        };

        if (_newNarrationInformationPairing == null)
        {
            _newNarrationInformationPairing = new NarrationInputParing[_newNarrationStrings.Length - 1];
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New String"))
            AddString();
        if (GUILayout.Button("Remove Last String"))
            RemoveString();
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < _newNarrationStrings.Length; i++)
        {


            EditorGUILayout.BeginVertical();


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("String " + (i + 1).ToString(), GUILayout.Width(50));
            _newNarrationStrings[i] = EditorGUILayout.TextField(_newNarrationStrings[i], GUILayout.ExpandWidth(true));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (i < _newNarrationInformationPairing.Length)
            {
                EditorGUILayout.LabelField("Information Type", GUILayout.Width(100));

                Rect enumRect = EditorGUILayout.BeginHorizontal();
                enumRect.width = Mathf.Min(enumRect.width, 150);

                _newNarrationInformationPairing[i].informationType = (NarrationInformationType)EditorGUILayout.EnumPopup(_newNarrationInformationPairing[i].informationType);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("Default Text", GUILayout.Width(100));

                EditorGUILayout.BeginHorizontal();

                _newNarrationInformationPairing[i].narrationString = EditorGUILayout.TextField(_newNarrationInformationPairing[i].narrationString, GUILayout.ExpandWidth(true));

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("GenerateTextPreview"))
        {
            _narrationPreviewString = CreateSampleString();
        }

        EditorGUILayout.LabelField(_narrationPreviewString, textWrapStyle);

        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Type of Narration", GUILayout.Width(125));
        _newNarrationType = (NarrationType)EditorGUILayout.EnumPopup(_newNarrationType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Asset Name", GUILayout.Width(100));
        _newNarrationName = EditorGUILayout.TextField(_newNarrationName);
        EditorGUILayout.EndHorizontal();

        SavingButtons();

        EditorGUILayout.EndVertical();
    }

    private void DrawViewAllNarrationsWindow() //draws the window for viewing all narrations. 
    {
        GUILayout.BeginArea(new Rect(0, 0, _narrationEditorWindowInstance.position.width * 0.5f, _narrationEditorWindowInstance.position.height));
        {
            DrawNewNarrationWindow();
        }
        GUILayout.EndArea();

        //draws divider line
        EditorGUI.DrawRect(new Rect(_narrationEditorWindowInstance.position.width * 0.5f - 2, 0, 4, _narrationEditorWindowInstance.position.height), Color.gray);

        GUILayout.BeginArea(new Rect(_narrationEditorWindowInstance.position.width * 0.5f, 0, _narrationEditorWindowInstance.position.width * 0.5f, _narrationEditorWindowInstance.position.height));
        if (GUILayout.Button("Update from Json"))
        {
            narrationDictionary = GetNarrations();
        }
        if (narrationDictionary != null)
        {
            DisplayNarrationsFromDictionary(narrationDictionary);
        }
        GUILayout.EndArea();
    }

    #region "Create New Display Adjustment Functions"
    private void AddString() //adds a string set from the window editor 
    {
        if(_newNarrationStrings == null)
        {
            _newNarrationStrings = new string[1];
            return;
        }
        if(_newNarrationStrings.Length > 4)
        {
            return;
        }


        string[] holderStrings = _newNarrationStrings;

        NarrationInputParing[] informationTypeHolders = _newNarrationInformationPairing;

        _newNarrationStrings = new string[holderStrings.Length + 1];

        _newNarrationInformationPairing = new NarrationInputParing[_newNarrationStrings.Length - 1];

        for (int i = 0; i < holderStrings.Length; i++)
        {
            _newNarrationStrings[i] = holderStrings[i];
            if(i < _newNarrationInformationPairing.Length)
            {
                if(i < informationTypeHolders.Length)
                {
                    _newNarrationInformationPairing[i] = informationTypeHolders[i];
                }

            }
        }


    }
    private void RemoveString() //removes a string set from the window editor 
    {
        if (_newNarrationStrings == null)
        {
            _newNarrationStrings = new string[0];
            return;

        }

        if(_newNarrationStrings.Length < 3)
        {
            return;
        }

        string[] holderStrings = _newNarrationStrings;

        NarrationInputParing[] informationTypeHolders = _newNarrationInformationPairing;

        _newNarrationStrings = new string[holderStrings.Length - 1];

        _newNarrationInformationPairing = new NarrationInputParing[_newNarrationStrings.Length - 1];

        for (int i = 0; i < _newNarrationStrings.Length; i++)
        {
            _newNarrationStrings[i] = holderStrings[i];
            if (i < _newNarrationInformationPairing.Length && i < informationTypeHolders.Length)
            {
                _newNarrationInformationPairing[i] = informationTypeHolders[i]; 
            }
        }
    }
    private string CreateSampleString() //creates a string from current inputs that simulates what it will appear as in game 
    {

        if(_newNarrationStrings != null)
        {
            string sampleString = "";
            for (int i = 0; i < _newNarrationStrings.Length; i++)
            {
                sampleString += _newNarrationStrings[i];

                if(i < _newNarrationInformationPairing.Length)
                {
                    sampleString += _newNarrationInformationPairing[i].narrationString;
                }

            }
            return sampleString;
        }
        else
        {
            return "Couldn't generate string";
        }
    }

    private void SavingButtons() //displays overwrite and delete buttons if the currently worked on narration exsists, otherwise displays save button
    {
        if (OverwriteCheck(CurrentToNarationData()))
        {
            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Overwrite"))
            {
                SaveNarration();
                narrationDictionary = GetNarrations();
            }
            if (GUILayout.Button("Delete"))
            {
                EraseNarration();
                narrationDictionary = GetNarrations();
            }

            GUILayout.EndHorizontal();
        }
        else
        {
            if (GUILayout.Button("Save Asset"))
            {
                SaveNarration();
                narrationDictionary = GetNarrations();
            }

        }
    }
    #endregion

    #region"View All Display Adjustment Functions"

    private void DisplayNarrationsFromDictionary(Dictionary<NarrationType, List<Narration>> narrations)
    {
        _dictionaryScroll = GUILayout.BeginScrollView(_dictionaryScroll);
        GUILayout.BeginHorizontal();
        foreach(int type in Enum.GetValues(typeof(NarrationType)))
        {
            if(narrations.TryGetValue((NarrationType)type, out List<Narration> narrationList))
            {
                DisplayCategory(narrationList, Enum.GetName(typeof(NarrationType), (NarrationType)type));
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    private void DisplayCategory(List<Narration> narrationList, string categoryName)
    {
        GUILayout.BeginVertical();
        GUILayout.Label(categoryName);

        for (int i = 0; i < narrationList.Count; i++)
        {
            DisplayNarration(narrationList[i]);
        }
        GUILayout.EndVertical();

    }

    private void DisplayNarration(Narration narration)
    {
        GUILayout.BeginHorizontal();
        if(GUILayout.Button(narration.narrationName))
        {
            NarrationToCurrent(narration);
        }
        GUILayout.EndHorizontal();
    }

    #endregion

    //narration saving
    #region"Narration Saving"
    private Narration CurrentToNarationData() //converts current values in inspector into narration data 
    {
        Narration newData = new Narration(_newNarrationName, _newNarrationType, _newNarrationInformationPairing, _newNarrationStrings); //set through the constructor
        return newData;
    }
    private void NarrationToCurrent(Narration narration)
    {
        if(narration != null)
        {
            _newNarrationName = narration.narrationName;
            _newNarrationType = narration.narrationType;
            _newNarrationInformationPairing = narration.narrationInputPairings;
            _newNarrationStrings = narration.narrationStructureStrings;
        }
    } //converts current data to inputted narration's

    private void SaveNarration() //using CurrentToNarationData, saves values to Json Additivly 
    {
        if(SaveValidate())
        {
            NarrationJsonConverter jsonConverter = new NarrationJsonConverter(_jsonFilePath);

            jsonConverter.SaveNarration(CurrentToNarationData());

            Debug.Log("Asset Saved Sucsessfully");
        }
        else
        {
            Debug.Log("Asset couldn't be saved, make sure all fields are filled out");
        }
    }

    private void EraseNarration() //using CurrentToNarationData, saves values to Json Additivly 
    {
        NarrationJsonConverter jsonConverter = new NarrationJsonConverter(_jsonFilePath);
        jsonConverter.TryDeleteNarration(CurrentToNarationData());
    }

    private Dictionary<NarrationType, List<Narration>> GetNarrations()
    {
        NarrationJsonConverter jsonConverter = new NarrationJsonConverter(_jsonFilePath);

        return jsonConverter.GetNarrationsAsGameFormat();
    } //gets the narrations from Json. 

    //validates that all fields have contents before allowing a save.
    private bool SaveValidate()
    {
        if(_newNarrationStrings != null && _newNarrationInformationPairing != null) //null check
        {
            if(_newNarrationStrings.Length == _newNarrationInformationPairing.Length + 1 && _newNarrationStrings.Length >= 2) //length check
            {
                for (int i = 0; i < _newNarrationInformationPairing.Length; i++)
                {
                    if(_newNarrationStrings[i] == "" || _newNarrationInformationPairing[i].narrationString == "") //string contents check
                    {
                        return false;
                    }
                    if(_newNarrationStrings[_newNarrationStrings.Length - 1] == "" || _newNarrationName == "") //string contents check
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        return false;
    }

    private bool OverwriteCheck(Narration newNarration)
    {
        Dictionary<NarrationType, List<Narration>> narrations = GetNarrations();
        List<Narration> listToCheck;

        if(narrations != null)
        {
            if(narrations.TryGetValue(newNarration.narrationType, out listToCheck))
            {
                for (int i = 0; i < listToCheck.Count; i++)
                {
                    if(listToCheck[i].narrationName == newNarration.narrationName)
                    {
                        //same type and name, overwrite
                        return true;
                    }
                }
            }
        }

        //isint the same type and name, don't overwrite
        return false;
    }
    #endregion
}
#endif


