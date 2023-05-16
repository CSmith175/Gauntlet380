using System.Collections;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    private int openTimer = 60;

    public bool IsOpened
    {
        get;
        private set;
    }

    private void OnEnable()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        IsOpened = false;
        StartCoroutine(CountdownToOpen());
    }

    public void OpenDoor()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        IsOpened = true;
        StopCoroutine(CountdownToOpen());
    }

    private IEnumerator CountdownToOpen()
    {
        yield return new WaitForSeconds(openTimer);
        OpenDoor();
    }
}
