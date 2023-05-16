using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    private TMP_Text textMesh;

    private Mesh mesh;
    private Vector3[] verticies;

    private Coroutine textScrolling;

    [Range(1f, 20f)] public float wobbleIntensity;
    [Range(0.01f, 0.1f)] public float scrollSpeed = 0.35f;
    [Tooltip("Amount of time the narration sticks around after being written")] [Range(2, 6)] public float narrationHangtime = 4;

    [Tooltip("The Maximum amount of narrations that can be buffered into the queue")] [SerializeField] private int _maxQueueLength = 3;
    private readonly Queue<string> _narrationQueue = new Queue<string>();

    [Header("For Narrator Image Animation")]
    [Space(10)]
    [SerializeField] private Image _narratorImage;
    [SerializeField] private Sprite _closedMouth;
    [SerializeField] private Sprite _openMouth;

    [Tooltip("Min time for the narrator to change face poses")] [SerializeField] private float _minNarratorDelay;
    [Tooltip("Max time for the narrator to change face poses")] [SerializeField] private float _maxNarratorDelay;
    float _currentTime = 0;
    float _currentDuration = 1;


    public void DisplayNarration(string narration)
    {
        if(textScrolling == null)
        {
            textScrolling = StartCoroutine(DisplayNarration(narration, scrollSpeed));
        }
        else
        {
            if(_narrationQueue.Count <= _maxQueueLength)
            {
                _narrationQueue.Enqueue(narration);
            }
        }
    }

    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        verticies = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            int vertexCount = mesh.vertices.Length;

            TMP_CharacterInfo character = textMesh.textInfo.characterInfo[i];

            int index = character.vertexIndex;

            if(!char.IsWhiteSpace(character.character))
            {
                Vector3 offset = Wobble(Time.time + i);
                verticies[index] += offset;
                verticies[index + 1] += offset;
                verticies[index + 2] += offset;
                verticies[index + 3] += offset;
            }


        }

        mesh.vertices = verticies;
        textMesh.canvasRenderer.SetMesh(mesh);

        if(_narratorImage && _openMouth && _closedMouth)
        {
            if(textScrolling != null)
            {
                if(_currentTime + _currentDuration < Time.time)
                {
                    _currentTime = Time.time;
                    _currentTime = Time.time + Random.Range(_minNarratorDelay, _maxNarratorDelay);

                    if (_narratorImage.sprite == _openMouth)
                        _narratorImage.sprite = _closedMouth;
                    else
                    {
                        _narratorImage.sprite = _openMouth;
                    }
                }
            }
            else
                _narratorImage.sprite = _closedMouth;
        }
    }

    private Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * wobbleIntensity), Mathf.Cos(time * wobbleIntensity));
    }

    private IEnumerator DisplayNarration(string narration, float speed)
    {
        WaitForSeconds delay = new WaitForSeconds(speed);
        textMesh.text = "";

        for (int i = 0; i < narration.Length; i++)
        {
            textMesh.text += narration[i];

            yield return delay;
        }

        yield return new WaitForSeconds(narrationHangtime);

        textScrolling = null;
        if (_narrationQueue.Count > 0)
            DisplayNarration(_narrationQueue.Dequeue());
        else
            textMesh.text = " ";

    }
}
