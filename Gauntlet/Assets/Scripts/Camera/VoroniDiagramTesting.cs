using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoroniDiagramTesting : MonoBehaviour
{
    [SerializeField] private int _playerAmount;
    [SerializeField] private RawImage _testPlayerVoroniImage;

    private int _voroniSize;
    private Vector2[] _playerPosArray;
    private Color[] _playerColors;

    bool _toggleConstantUpdate = true;

    [SerializeField] private Color[] colors;
    private void Update()
    {
        if(_toggleConstantUpdate)
        {
            RefreshVoroni(_playerAmount);
        }
    }

    private void Awake()
    {
        RefreshVoroni(_playerAmount);
    }

    public void RefreshVoroni(int playerAmount)
    {
        _voroniSize = Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.x);
        RandomizePlayerInformation(playerAmount);
        GenerateDiagram();
    }

    private void GenerateDiagram()
    {
        Texture2D voroniTexture = new Texture2D(_voroniSize, _voroniSize);

        for (int x = 0; x < _voroniSize; x++)
        {
            for (int y = 0; y < _voroniSize; y++)
            {
                float closestPlayerDistance = int.MaxValue;
                float currentDistance;
                int closestPlayerIndex = 0;
                Vector2 pixelVector;

                //figures out which player pixel is closest to
                for (int i = 0; i < _playerPosArray.Length; i++)
                {
                    pixelVector.x = x;
                    pixelVector.y = y;
                    currentDistance = Vector2.Distance(_playerPosArray[i], pixelVector);
                    if (currentDistance < closestPlayerDistance)
                    {
                        closestPlayerDistance = currentDistance;
                        closestPlayerIndex = i;
                    }
                }


                voroniTexture.SetPixel(x, y, _playerColors[closestPlayerIndex]);
            }
        }

        voroniTexture.Apply();
        _testPlayerVoroniImage.texture = voroniTexture;
    }

    private void RandomizePlayerInformation(int playerAmount)
    {
        _playerPosArray = new Vector2[playerAmount];
        _playerColors = new Color[playerAmount];

        int indexer = 0;

        for (int i = 0; i < playerAmount; i++)
        {
            _playerPosArray[i] = new Vector2(Random.Range(0, _voroniSize), Random.Range(0, _voroniSize));

            
            if(colors != null)
            {

                if (indexer >= colors.Length)
                    indexer = 0;

                _playerColors[i] = colors[indexer];
                indexer++;
            }
            else
            {
                _playerColors[i] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
            }

        }
    }

    private void OnGUI()
    {
        /*
        GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, 100));
        GUILayout.BeginVertical();
        
        if(GUILayout.Button("Reroll Voroni"))
        {
            RefreshVoroni(_playerAmount);
        }
        if (GUILayout.Button("Toggle Constant Refresh"))
        {
            _toggleConstantUpdate = !_toggleConstantUpdate;
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
        */
    }
}
