using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class NarrationEditor : EditorWindow
{
    private NarrationType _newAssetNarrationType;
    private string[] _newAssetStrings = new string[2];

    private static EditorWindow _narrationEditorWindowInstance;
    private string _sampleString;



    [MenuItem("Narration/NarrationEditor")]
    public static void ShowNarrationEditorWindow()
    {
        _narrationEditorWindowInstance = GetWindow<NarrationEditor>("Narration Asset Editor");
        _narrationEditorWindowInstance.minSize = new Vector2Int(300, 300);


    }

    private void OnGUI()
    {
        #region "Styles"
        GUIStyle textWrapStyle = new GUIStyle(GUI.skin.GetStyle("label"))
        {
            wordWrap = true
        };
        #endregion

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New String"))
            AddString();
        if (GUILayout.Button("Remove Last String"))
            RemoveString();
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < _newAssetStrings.Length; i++)
        {

            float width = EditorGUILayout.BeginHorizontal().width;

            EditorGUILayout.LabelField("String " + (i + 1).ToString(), GUILayout.Width(50));



            _newAssetStrings[i] = EditorGUILayout.TextField(_newAssetStrings[i], GUILayout.ExpandWidth(true));

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        if(GUILayout.Button("GenerateTextPreview"))
        {
            _sampleString = CreateSampleString();
        }



        EditorGUILayout.LabelField(_sampleString, textWrapStyle);

        EditorGUILayout.EndVertical();
    }

    private void AddString()
    {
        if(_newAssetStrings == null)
        {
            _newAssetStrings = new string[1];
            return;
        }
        if(_newAssetStrings.Length > 4)
        {
            return;
        }


        string[] holderStrings = _newAssetStrings;
        _newAssetStrings = new string[holderStrings.Length + 1];

        for (int i = 0; i < holderStrings.Length; i++)
        {
            _newAssetStrings[i] = holderStrings[i];
        }

    }
    private void RemoveString()
    {
        if (_newAssetStrings == null)
        {
            _newAssetStrings = new string[0];
            return;
        }

        if(_newAssetStrings.Length < 3)
        {
            return;
        }

        string[] holderStrings = _newAssetStrings;
        _newAssetStrings = new string[holderStrings.Length - 1];

        for (int i = 0; i < _newAssetStrings.Length; i++)
        {
            _newAssetStrings[i] = holderStrings[i];
        }
    }

    private string CreateSampleString()
    {

        if(_newAssetStrings != null)
        {
            string sampleString = "";
            for (int i = 0; i < _newAssetStrings.Length; i++)
            {
                sampleString += _newAssetStrings[i];

                if(i != _newAssetStrings.Length - 1) //dosent add if its the last one.
                {
                    sampleString += " X ";
                }

            }
            return sampleString;
        }
        else
        {
            return "Couldn't generate string";
        }
    }


}
