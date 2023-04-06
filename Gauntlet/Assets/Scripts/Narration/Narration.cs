using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNarration", menuName = "Narration")]
public class Narration : ScriptableObject
{
    [HideInInspector] [SerializeField] private string[] _narrationTextChunck;
    [HideInInspector] [SerializeField] private AudioClip _narrationAudio;
}
