using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OpenDoor()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
