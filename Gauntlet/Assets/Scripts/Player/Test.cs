using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestFunc(NarrationType.PlayerJoined & NarrationType.Undefined & NarrationType.FoodPickedUp);
    }
    private void TestFunc(NarrationType type)
    {
        Debug.Log(type);
    }
}
