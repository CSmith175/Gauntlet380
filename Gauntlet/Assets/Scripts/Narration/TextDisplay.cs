using System.Collections;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    private TMP_Text textMesh;

    private Mesh mesh;
    private Vector3[] verticies;

    private Coroutine textScrolling;

    [Range(1f, 20f)] public float wobbleIntensity;
    [Range(0.01f, 0.1f)] public float scrollSpeed = 0.35f;


    public void DisplayNarration(string narration)
    {
        if(textScrolling == null)
        {
            textScrolling = StartCoroutine(DisplayNarration(narration, scrollSpeed));
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

        textScrolling = null;
    }
}
